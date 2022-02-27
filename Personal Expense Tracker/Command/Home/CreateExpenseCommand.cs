using System;
using System.ComponentModel;
using System.Windows;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class CreateExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public CreateExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _homeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_homeViewModel.ExpenseName) &&
                !string.IsNullOrWhiteSpace(_homeViewModel.ExpenseAmount) &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedCategory != null)
            {
                try
                {
                    DateTime expenseDate = _homeViewModel.ExpenseDate;
                    string expenseName = _formattingService.FormatExpenseName(_homeViewModel.ExpenseName);
                    double expenseAmount = _formattingService.FormatExpenseAmount(_homeViewModel.ExpenseAmount);

                    int newExpenseId = _databaseService.InsertExpense
                    (
                        _homeViewModel.SelectedCategory.Name,
                        _formattingService.FormatExpenseDate(expenseDate),
                        expenseName,
                        expenseAmount
                    );

                    if ((_homeViewModel.GroupByMonth && expenseDate.Month == _homeViewModel.SelectedMonth+1 && expenseDate.Year == _homeViewModel.SelectedYear) ||
                        (!_homeViewModel.GroupByMonth && expenseDate.Year == _homeViewModel.SelectedYear))
                    {
                        _homeViewModel.ExpenseCollection.Add(new ExpenseViewModel(new Expense
                        (
                            newExpenseId,
                            expenseDate,
                            expenseName,
                            expenseAmount
                        )));
                    }

                    _homeViewModel.AddYear(expenseDate.Year);

                    _homeViewModel.ExpenseName = string.Empty;
                    _homeViewModel.ExpenseAmount = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HomeViewModel.ExpenseDate) ||
                e.PropertyName == nameof(HomeViewModel.ExpenseName) ||
                e.PropertyName == nameof(HomeViewModel.ExpenseAmount))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
