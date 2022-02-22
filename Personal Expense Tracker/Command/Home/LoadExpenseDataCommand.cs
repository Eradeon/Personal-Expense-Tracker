using System;
using System.Data;
using System.Linq;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Model;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class LoadExpenseDataCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;

        public LoadExpenseDataCommand(MainViewModel mainViewModel, DatabaseService databaseService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_mainViewModel.SelectedCategory != null)
            {
                _mainViewModel.ExpenseCollection.Clear();

                if (_mainViewModel.GroupByMonth)
                {
                    DataTable dataTable = _databaseService.QueryDatabase(_databaseService.BuildExpenseQuery
                    (
                        _mainViewModel.SelectedCategory.Name,
                        _mainViewModel.SelectedYear.ToString(),
                        _mainViewModel.MonthList.ElementAt(_mainViewModel.SelectedMonth).Key
                    ));

                    LoadExpenses(dataTable);
                }
                else
                {
                    DataTable dataTable = _databaseService.QueryDatabase(_databaseService.BuildExpenseQuery
                    (
                        _mainViewModel.SelectedCategory.Name,
                        _mainViewModel.SelectedYear.ToString(),
                        string.Empty
                    ));

                    LoadExpenses(dataTable);
                }
            }
        }

        private void LoadExpenses(DataTable dataTable)
        {
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                    _mainViewModel.LoadingExpenses = true;

                int index = 1;

                foreach (DataRow row in dataTable.Rows)
                {
                    if (index == dataTable.Rows.Count)
                    {
                        _mainViewModel.LoadingExpenses = false;
                    }

                    _mainViewModel.ExpenseCollection.Add(new ExpenseViewModel(new Expense
                    (
                        int.Parse(row["expense_id"].ToString()),
                        row["expense_date"].ToString(),
                        row["expense_name"].ToString(),
                        double.Parse(row["expense_amount"].ToString())
                    )));

                    index++;
                }
            }
        }
    }
}
