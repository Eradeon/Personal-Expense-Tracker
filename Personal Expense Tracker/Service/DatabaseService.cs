using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Text;
using System.Windows;

namespace Personal_Expense_Tracker.Service
{
    internal class DatabaseService
    {
        private readonly string _databaseLocation;
        private readonly string _databaseName;
        private readonly string _dataSource;

        public DatabaseService()
        {
            _databaseLocation = ".\\data\\";
            _databaseName = "appdata.db";
            _dataSource = string.Concat("Data Source=", _databaseLocation, _databaseName);
        }

        #region Properties
        public string DatabaseLocation
        {
            get { return _databaseLocation; }
        }
        #endregion Properties

        public bool DatabaseExists()
        {
            return File.Exists(string.Concat(_databaseLocation, _databaseName));
        }

        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_databaseLocation + _databaseName);
        }

        #region Create Table
        //App Data
        public void CreateCategoryTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS categories (category_id INTEGER PRIMARY KEY AUTOINCREMENT, category_name TEXT NOT NULL, category_display_name TEXT NOT NULL, group_by_month INTEGER NOT NULL);";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        //Expense
        public void CreateExpenseTable(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (expense_id INTEGER PRIMARY KEY AUTOINCREMENT, expense_date TEXT NOT NULL, expense_name TEXT NOT NULL, expense_amount DOUBLE NOT NULL);";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion Create Table

        #region Insert
        //App Data
        public void InsertDefaultCategory(string categoryName, string categoryDisplayName, int group_by_month)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO categories (category_name, category_display_name, group_by_month) VALUES (@name,@displayName,@groupByMonth)";
                    command.Parameters.Add(new SQLiteParameter("@name", categoryName));
                    command.Parameters.Add(new SQLiteParameter("@displayName", categoryDisplayName));
                    command.Parameters.Add(new SQLiteParameter("@groupByMonth", group_by_month));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public int InsertCategory(string categoryName, string categoryDisplayName, int group_by_month)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO categories (category_name, category_display_name, group_by_month) VALUES (@name,@displayName,@groupByMonth)";
                    command.Parameters.Add(new SQLiteParameter("@name", categoryName));
                    command.Parameters.Add(new SQLiteParameter("@displayName", categoryDisplayName));
                    command.Parameters.Add(new SQLiteParameter("@groupByMonth", group_by_month));

                    connection.Open();
                    command.ExecuteNonQuery();

                    return Convert.ToInt32(connection.LastInsertRowId);
                }
            }
        }

        public int InsertExpense(string tableName, string expenseDate, string expenseName, double expenseAmount)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"INSERT INTO {tableName} (expense_date, expense_name, expense_amount) VALUES (@date,@name,@amount);";
                    command.Parameters.Add(new SQLiteParameter("@date", expenseDate));
                    command.Parameters.Add(new SQLiteParameter("@name", expenseName));
                    command.Parameters.Add(new SQLiteParameter("@amount", expenseAmount));

                    connection.Open();
                    command.ExecuteNonQuery();

                    return Convert.ToInt32(connection.LastInsertRowId);
                }
            }
        }

        //Expense
        #endregion Insert

        #region Delete
        public void DeleteCategory(int id, string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = $"DELETE FROM categories WHERE category_id = {id};";
                    command.ExecuteNonQuery();

                    command.CommandText = $"DROP TABLE IF EXISTS {tableName};";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteExpense(int id, string tableName)
        {
            using (SQLiteConnection connection= new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"DELETE FROM {tableName} WHERE expense_id = {id};";

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion Delete

        #region Update
        public void RenameTable(int id, string existingTableName, string newTableName, string newDisplayName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = $"ALTER TABLE {existingTableName} RENAME TO {newTableName};";
                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE categories SET category_name = @newTableName, category_display_name = @newDisplayName WHERE category_id = @id;";
                    command.Parameters.Add(new SQLiteParameter("@newTableName", newTableName));
                    command.Parameters.Add(new SQLiteParameter("@newDisplayName", newDisplayName));
                    command.Parameters.Add(new SQLiteParameter("@id", id));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateExpense(int id, string tableName, string newExpenseDate, string newExpenseName, double newExpenseAmount)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"UPDATE {tableName} SET expense_date = @newExpenseDate, expense_name = @newExpenseName, expense_amount = @nexExpenseAmount WHERE expense_id = @id;";
                    command.Parameters.Add(new SQLiteParameter("newExpenseDate", newExpenseDate));
                    command.Parameters.Add(new SQLiteParameter("newExpenseName", newExpenseName));
                    command.Parameters.Add(new SQLiteParameter("nexExpenseAmount", newExpenseAmount));
                    command.Parameters.Add(new SQLiteParameter("id", id));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateGroupByMonth(int id, bool newValue)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_dataSource))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE categories SET group_by_month = @groupByMonth WHERE category_id = @id;";
                    command.Parameters.Add(new SQLiteParameter("groupByMonth", newValue));
                    command.Parameters.Add(new SQLiteParameter("id", id));

                    connection.Open();
                    command.ExecuteNonQuery();
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    return dataTable;
                }
            }
        }

        public string BuildExpenseQuery(string tableName, string year, string month)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM ");
            query.Append(tableName);
            query.Append(" WHERE strftime('%Y', expense_date) = '");
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