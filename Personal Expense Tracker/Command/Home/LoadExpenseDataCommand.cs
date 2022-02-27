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
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;

        public LoadExpenseDataCommand(HomeViewModel homeViewModel, DatabaseService databaseService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedCategory != null)
            {
                _homeViewModel.ExpenseCollection.Clear();

                if (_homeViewModel.GroupByMonth)
                {
                    DataTable dataTable = _databaseService.QueryDatabase(_databaseService.BuildExpenseQuery
                    (
                        _homeViewModel.SelectedCategory.Name,
                        _homeViewModel.SelectedYear.ToString(),
                        _homeViewModel.MonthList.ElementAt(_homeViewModel.SelectedMonth).Key
                    ));

                    LoadExpenses(dataTable);
                }
                else
                {
                    DataTable dataTable = _databaseService.QueryDatabase(_databaseService.BuildExpenseQuery
                    (
                        _homeViewModel.SelectedCategory.Name,
                        _homeViewModel.SelectedYear.ToString(),
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
                    _homeViewModel.LoadingExpenses = true;

                int index = 1;

                foreach (DataRow row in dataTable.Rows)
                {
                    if (index == dataTable.Rows.Count)
                    {
                        _homeViewModel.LoadingExpenses = false;
                    }

                    _homeViewModel.ExpenseCollection.Add(new ExpenseViewModel(new Expense
                    (
                        int.Parse(row["expense_id"].ToString()),
                        DateTime.Parse(row["expense_date"].ToString()),
                        row["expense_name"].ToString(),
                        double.Parse(row["expense_amount"].ToString())
                    )));

                    index++;
                }
            }
        }
    }
}
