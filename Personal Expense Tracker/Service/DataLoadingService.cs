using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Extension;
using System.Collections.Generic;
using System.Globalization;

namespace Personal_Expense_Tracker.Service
{
    internal class DataLoadingService
    {
        private DatabaseService _databaseService;
        private FormattingService _formattingService;

        public DataLoadingService(DatabaseService databaseService, FormattingService formattingService)
        {
            _databaseService = databaseService;
            _formattingService = formattingService;
        }

        public void CreateDefaultDatabase()
        {
            if (!_databaseService.DatabaseExists())
            {
                Directory.CreateDirectory(_databaseService.DatabaseLocation);
                _databaseService.CreateDatabase();
                _databaseService.CreateCategoryTable();

                string displayName = "Potraviny";
                string tableName = _formattingService.FormatCategoryTableName(displayName);
                _databaseService.CreateExpenseTable(tableName);
                _databaseService.InsertDefaultCategory(tableName, displayName, true.ToInt());

                displayName = "Toby";
                tableName = _formattingService.FormatCategoryTableName(displayName);
                _databaseService.CreateExpenseTable(tableName);
                _databaseService.InsertDefaultCategory(tableName, displayName, false.ToInt());
            }
        }

        public ObservableCollection<CategoryViewModel> LoadCategories()
        {
            DataTable dataTable = _databaseService.QueryDatabase("SELECT * FROM categories;");
            ObservableCollection<CategoryViewModel> categoryCollection = new ObservableCollection<CategoryViewModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                categoryCollection.Add(new CategoryViewModel(new Category
                (
                    int.Parse(row["category_id"].ToString()),
                    row["category_name"].ToString(),
                    row["category_display_name"].ToString(),
                    int.Parse(row["group_by_month"].ToString()).ToBool()
                )));
            }

            return categoryCollection;
        }
    }
}
