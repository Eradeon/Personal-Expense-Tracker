using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class EditExpenseCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public EditExpenseCommand(MainViewModel mainViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            if (_mainViewModel.EditExpenseConfirmation)
            {
                return !string.IsNullOrWhiteSpace(_mainViewModel.EditExpenseName) &&
                !string.IsNullOrWhiteSpace(_mainViewModel.EditExpenseAmount) &&
                base.CanExecute(parameter);
            }
            else { return true && base.CanExecute(parameter); }
                
        }

        public override void Execute(object? parameter)
        {
            if (_mainViewModel.SelectedRow != null && _mainViewModel.SelectedCategory != null)
            {
                if (_mainViewModel.EditExpenseConfirmation)
                {
                    string newExpenseDate = _formattingService.FormatExpenseDate(_mainViewModel.EditExpenseDate);
                    string newExpenseName = _formattingService.FormatExpenseName(_mainViewModel.EditExpenseName);
                    double newExpenseAmount = _formattingService.FormatExpenseAmount(_mainViewModel.EditExpenseAmount);

                    _databaseService.UpdateExpense
                    (
                        _mainViewModel.SelectedRow.Id,
                        _mainViewModel.SelectedCategory.Name,
                        newExpenseDate,
                        newExpenseName,
                        newExpenseAmount
                    );

                    foreach (var expense in _mainViewModel.ExpenseCollection)
                    {
                        if (expense.Id == _mainViewModel.SelectedRow.Id)
                        {
                            expense.Date = newExpenseDate;
                            expense.Name = newExpenseName;
                            expense.Amount = newExpenseAmount;
                        }
                    }

                    _mainViewModel.EditExpenseName = string.Empty;
                    _mainViewModel.EditExpenseAmount = string.Empty;

                    _mainViewModel.EditExpenseConfirmation = false;
                    _mainViewModel.EditExpenseModalVisible = false;
                }
                else
                {
                    _mainViewModel.EditExpenseDate = DateTime.Parse(_mainViewModel.SelectedRow.Date);
                    _mainViewModel.EditExpenseName = _mainViewModel.SelectedRow.Name;
                    _mainViewModel.EditExpenseAmount = _mainViewModel.SelectedRow.Amount.ToString();

                    _mainViewModel.EditExpenseConfirmation = true;
                    _mainViewModel.EditExpenseModalVisible = true;
                }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainViewModel.EditExpenseConfirmation) ||
                e.PropertyName == nameof(_mainViewModel.EditExpenseDate) ||
                e.PropertyName == nameof(_mainViewModel.EditExpenseName) ||
                e.PropertyName == nameof(_mainViewModel.EditExpenseAmount))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
