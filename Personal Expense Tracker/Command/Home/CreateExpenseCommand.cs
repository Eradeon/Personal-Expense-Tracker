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
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public CreateExpenseCommand(MainViewModel mainViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_mainViewModel.ExpenseName) &&
                !string.IsNullOrWhiteSpace(_mainViewModel.ExpenseAmount) &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (_mainViewModel.SelectedCategory != null)
            {
                try
                {
                    DateTime expenseDate = _mainViewModel.ExpenseDate;
                    string expenseName = _formattingService.FormatExpenseName(_mainViewModel.ExpenseName);
                    double expenseAmount = _formattingService.FormatExpenseAmount(_mainViewModel.ExpenseAmount);

                    int newExpenseId = _databaseService.InsertExpense
                    (
                        _mainViewModel.SelectedCategory.Name,
                        _formattingService.FormatExpenseDate(expenseDate),
                        expenseName,
                        expenseAmount
                    );

                    if ((_mainViewModel.GroupByMonth && expenseDate.Month == _mainViewModel.SelectedMonth+1 && expenseDate.Year == _mainViewModel.SelectedYear) ||
                        (!_mainViewModel.GroupByMonth && expenseDate.Year == _mainViewModel.SelectedYear))
                    {
                        _mainViewModel.ExpenseCollection.Add(new ExpenseViewModel(new Expense
                        (
                            newExpenseId,
                            expenseDate,
                            expenseName,
                            expenseAmount
                        )));
                    }

                    _mainViewModel.ExpenseName = string.Empty;
                    _mainViewModel.ExpenseAmount = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.ExpenseDate) ||
                e.PropertyName == nameof(MainViewModel.ExpenseName) ||
                e.PropertyName == nameof(MainViewModel.ExpenseAmount))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
