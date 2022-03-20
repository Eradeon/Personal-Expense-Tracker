using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Service
{
    internal enum DatabaseActionResult
    {
        Success,
        Error
    }

    internal class DatabaseService
    {
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        private readonly string _databaseLocation;
        private readonly string _databaseName;
        private readonly string _dataSource;

        public DatabaseService(FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _formattingService = formattingService;
            _messageBoxStore = messageBoxStore;

            _databaseLocation = ".\\data\\";
            _databaseName = "appdata.db";
            _dataSource = string.Concat("Data Source=", _databaseLocation, _databaseName);
        }

        public void CreateDefaultDatabase()
        {
            if (!File.Exists(string.Concat(_databaseLocation, _databaseName)))
            {
                Directory.CreateDirectory(_databaseLocation);
                SQLiteConnection.CreateFile(_databaseLocation + _databaseName);
                if (CreateCategoriesTable() == DatabaseActionResult.Error)
                    return;

                string displayName = "Potraviny";
                string tableName = _formattingService.FormatCategoryTableName(displayName);
                CreateExpenseCategory(tableName, displayName, true.ToInt());

                displayName = "Toby";
                tableName = _formattingService.FormatCategoryTableName(displayName);
                CreateExpenseCategory(tableName, displayName, false.ToInt());
            }
        }

        #region Create Table
        //App Data
        private DatabaseActionResult CreateCategoriesTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS categories (category_id INTEGER PRIMARY KEY AUTOINCREMENT, category_name TEXT NOT NULL, category_display_name TEXT NOT NULL, group_by_month INTEGER NOT NULL);";

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return DatabaseActionResult.Success;
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Při vytváření databáze došlo k chybě. Prosím, odstraňte soubor \"\\data\\appdata.db\" a restartujte apliakci.");
                        return DatabaseActionResult.Error;
                    }
                    
                }
            }
        }

        //Expense
        public int CreateExpenseCategory(string categoryTableName, string categoryDisplayName, int groupByMonth)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                try
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("", connection, transaction))
                        {
                            command.CommandType = CommandType.Text;

                            try
                            {
                                command.CommandText =
                                    $"CREATE TABLE IF NOT EXISTS \"{categoryTableName}\" (expense_id INTEGER PRIMARY KEY AUTOINCREMENT, expense_date TEXT NOT NULL, expense_name TEXT NOT NULL, expense_amount DOUBLE NOT NULL);";
                                command.ExecuteNonQuery();

                                command.CommandText =
                                    "INSERT INTO categories (category_name, category_display_name, group_by_month) VALUES (@tableName,@displayName,@groupByMonth)";
                                command.Parameters.Add(new SQLiteParameter("@tableName", categoryTableName));
                                command.Parameters.Add(new SQLiteParameter("@displayName", categoryDisplayName));
                                command.Parameters.Add(new SQLiteParameter("@groupByMonth", groupByMonth));
                                command.ExecuteNonQuery();

                                int result = Convert.ToInt32(connection.LastInsertRowId);

                                transaction.Commit();
                                return result;
                            }
                            catch
                            {
                                try
                                {
                                    transaction.Rollback();
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při vytváření kategorie \"{categoryDisplayName}\" došlo k chybě.");
                                    return 0;
                                }
                                catch
                                {
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při vytváření kategorie \"{categoryDisplayName}\" došlo k chybě.\nDošlo k chybě při komunikaci s databází.");
                                    return 0;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Error, "Došlo k chybě při komunikaci s databází.");
                    return 0;
                }
            }
        }
        #endregion Create Table

        public int InsertExpense(string tableName, string expenseDate, string expenseName, double expenseAmount)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        $"INSERT INTO \"{tableName}\" (expense_date, expense_name, expense_amount) VALUES (@date,@name,@amount);";
                    command.Parameters.Add(new SQLiteParameter("@date", expenseDate));
                    command.Parameters.Add(new SQLiteParameter("@name", expenseName));
                    command.Parameters.Add(new SQLiteParameter("@amount", expenseAmount));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        return Convert.ToInt32(connection.LastInsertRowId);
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Při vkládání záznamu do databáze došlo k chybě.");
                        return 0;
                    }
                }
            }
        }

        public List<ExpenseViewModel>? DuplicateExpenses(string categoryTableName, List<ExpenseViewModel> itemsToDuplicate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                try
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("", connection, transaction))
                        {
                            command.CommandType = CommandType.Text;

                            try
                            {
                                List<ExpenseViewModel> result = new();

                                foreach (var item in itemsToDuplicate)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText =
                                        $"INSERT INTO \"{categoryTableName}\" (expense_date, expense_name, expense_amount) VALUES (@date,@name,@amount);";
                                    command.Parameters.Add(new SQLiteParameter("@date", _formattingService.FormatExpenseDate(item.Date)));
                                    command.Parameters.Add(new SQLiteParameter("@name", item.Name));
                                    command.Parameters.Add(new SQLiteParameter("@amount", item.Amount));
                                    command.ExecuteNonQuery();

                                    result.Add(new ExpenseViewModel(new Expense
                                    (
                                        Convert.ToInt32(connection.LastInsertRowId),
                                        item.Date,
                                        item.Name,
                                        item.Amount,
                                        false,
                                        false
                                    )));
                                }

                                transaction.Commit();
                                return result;
                            }
                            catch
                            {
                                try
                                {
                                    transaction.Rollback();
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při duplikování výdajů došlo k chybě.");
                                    return null;
                                }
                                catch
                                {
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při duplikování výdajů došlo k chybě.\nDošlo k chybě při komunikaci s databází.");
                                    return null;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Error, "Došlo k chybě při komunikaci s databází.");
                    return null;
                }
            }
        }

        #region Delete
        public DatabaseActionResult DeleteCategory(int categoryId, string categoryTableName, string categoryDisplayName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                try
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("", connection, transaction))
                        {
                            command.CommandType = CommandType.Text;

                            try
                            {
                                command.CommandText =
                                    $"DELETE FROM categories WHERE category_id = {categoryId};";
                                command.ExecuteNonQuery();

                                command.CommandText =
                                    $"DROP TABLE IF EXISTS \"{categoryTableName}\";";
                                command.ExecuteNonQuery();

                                transaction.Commit();
                                return DatabaseActionResult.Success;
                            }
                            catch
                            {
                                try
                                {
                                    transaction.Rollback();
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při odstraňování kategorie \"{categoryDisplayName}\" došlo k chybě.");
                                    return DatabaseActionResult.Error;
                                }
                                catch
                                {
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při odstraňování kategorie \"{categoryDisplayName}\" došlo k chybě.\nDošlo k chybě při komunikaci s databází.");
                                    return DatabaseActionResult.Error;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Error, "Došlo k chybě při komunikaci s databází.");
                    return DatabaseActionResult.Error;
                }
            }
        }

        public DatabaseActionResult DeleteExpense(string categoryTableName, List<ExpenseViewModel> itemsToDelete)
        {
            using (SQLiteConnection connection= new SQLiteConnection(_dataSource))
            {
                try
                {
                    connection.Open();

                    using (SQLiteTransaction transaction= connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandType = CommandType.Text;

                            try
                            {
                                foreach (var item in itemsToDelete)
                                {
                                    command.CommandText =
                                        $"DELETE FROM \"{categoryTableName}\" WHERE expense_id = {item.Id};";
                                    command.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                return DatabaseActionResult.Success;
                            }
                            catch
                            {
                                try
                                {
                                    transaction.Rollback();
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při odstraňování výdajů došlo k chybě.");
                                    return DatabaseActionResult.Error;
                                }
                                catch
                                {
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při odstraňování výdajů došlo k chybě.\nDošlo k chybě při komunikaci s databází.");
                                    return DatabaseActionResult.Error;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Error, "Došlo k chybě při komunikaci s databází.");
                    return DatabaseActionResult.Error;
                }
            }
        }
        #endregion Delete

        #region Update
        public DatabaseActionResult RenameTable(int id, string existingTableName, string newTableName, string newDisplayName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                try
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandType = CommandType.Text;

                            try
                            {
                                command.CommandText = $"ALTER TABLE \"{existingTableName}\" RENAME TO \"{newTableName}\";";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE categories SET category_name = @newTableName, category_display_name = @newDisplayName WHERE category_id = @id;";
                                command.Parameters.Add(new SQLiteParameter("@newTableName", newTableName));
                                command.Parameters.Add(new SQLiteParameter("@newDisplayName", newDisplayName));
                                command.Parameters.Add(new SQLiteParameter("@id", id));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                                return DatabaseActionResult.Success;
                            }
                            catch
                            {
                                try
                                {
                                    transaction.Rollback();
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při přejmenovávání databáze došlo k chybě.");
                                    return DatabaseActionResult.Error;
                                }
                                catch
                                {
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při přejmenovávání databáze došlo k chybě.\nDošlo k chybě při komunikaci s databází.");
                                    return DatabaseActionResult.Error;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Error, "Došlo k chybě při komunikaci s databází.");
                    return DatabaseActionResult.Error;
                }
            }
        }

        public DatabaseActionResult UpdateExpense(int id, string tableName, string newExpenseDate, string newExpenseName, double newExpenseAmount)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        $"UPDATE \"{tableName}\" SET expense_date = @newExpenseDate, expense_name = @newExpenseName, expense_amount = @nexExpenseAmount WHERE expense_id = @id;";
                    command.Parameters.Add(new SQLiteParameter("newExpenseDate", newExpenseDate));
                    command.Parameters.Add(new SQLiteParameter("newExpenseName", newExpenseName));
                    command.Parameters.Add(new SQLiteParameter("nexExpenseAmount", newExpenseAmount));
                    command.Parameters.Add(new SQLiteParameter("id", id));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        return DatabaseActionResult.Success;
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Při úpravě záznamu v databázi došlo k chybě.");
                        return DatabaseActionResult.Error;
                    }
                }
            }
        }

        public DatabaseActionResult UpdateGroupByMonth(int id, bool newValue)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        "UPDATE categories SET group_by_month = @groupByMonth WHERE category_id = @id;";
                    command.Parameters.Add(new SQLiteParameter("groupByMonth", newValue));
                    command.Parameters.Add(new SQLiteParameter("id", id));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        return DatabaseActionResult.Success;
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Při aktualizaci nastavení kategorie došlo k chybě.");
                        return DatabaseActionResult.Error;
                    }
                }
            }
        }
        #endregion Update

        public DataTable QueryDatabase(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();

                    try
                    {
                        dataAdapter.Fill(dataTable);
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Při čtení dat z databáze došlo k chybě.");
                    }

                    return dataTable;
                }
            }
        }

        public DatabaseActionResult MergeTables(string fromTableName, string toTableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        $"INSERT INTO \"{toTableName}\" (expense_date, expense_name, expense_amount) SELECT expense_date,expense_name,expense_amount FROM \"{fromTableName}\";";

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        return DatabaseActionResult.Success;
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Při slučování kategorií došlo k chybě.");
                        return DatabaseActionResult.Error;
                    }
                }
            }
        }

        public DatabaseActionResult MoveRecords(string fromTableName, string toTableName, List<ExpenseViewModel> itemsToMove)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                try
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("", connection, transaction))
                        {
                            command.CommandType = CommandType.Text;

                            try
                            {
                                foreach (var item in itemsToMove)
                                {
                                    command.Parameters.Clear();

                                    command.CommandText =
                                        $"DELETE FROM \"{fromTableName}\" WHERE expense_id = {item.Id};";
                                    command.ExecuteNonQuery();

                                    command.CommandText =
                                        $"INSERT INTO \"{toTableName}\" (expense_date, expense_name, expense_amount) VALUES (@date,@name,@amount);";
                                    command.Parameters.Add(new SQLiteParameter("@date", _formattingService.FormatExpenseDate(item.Date)));
                                    command.Parameters.Add(new SQLiteParameter("@name", item.Name));
                                    command.Parameters.Add(new SQLiteParameter("@amount", item.Amount));
                                    command.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                return DatabaseActionResult.Success;
                            }
                            catch
                            {
                                try
                                {
                                    transaction.Rollback();
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při přesouvání výdajů došlo k chybě.");
                                    return DatabaseActionResult.Error;
                                }
                                catch
                                {
                                    _messageBoxStore.ShowMessageBox(MessageType.Error, $"Při přesouvání výdajů došlo k chybě.\nDošlo k chybě při komunikaci s databází.");
                                    return DatabaseActionResult.Error;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Error, "Došlo k chybě při komunikaci s databází.");
                    return DatabaseActionResult.Error;
                }
            }
        }

        public string BuildExpenseQuery(string tableName, string year, string month)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM \"");
            query.Append(tableName);
            query.Append("\" WHERE strftime('%Y', expense_date) = '");
            query.Append(year);
            
            if (month != null && !string.IsNullOrEmpty(month))
            {
                query.Append("' AND strftime('%m', expense_date) = '");
                query.Append(month);
            }

            query.Append("';");

            return query.ToString();
        }
    }
}