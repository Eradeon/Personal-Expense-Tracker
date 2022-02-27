using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class EditExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public EditExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _homeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            if (_homeViewModel.EditExpenseConfirmation)
            {
                return !string.IsNullOrWhiteSpace(_homeViewModel.EditExpenseName) &&
                !string.IsNullOrWhiteSpace(_homeViewModel.EditExpenseAmount) &&
                base.CanExecute(parameter);
            }
            else { return true && base.CanExecute(parameter); }
                
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedRow != null && _homeViewModel.SelectedCategory != null)
            {
                if (_homeViewModel.EditExpenseConfirmation)
                {
                    DateTime newExpenseDate = _homeViewModel.EditExpenseDate;
                    string newExpenseName = _formattingService.FormatExpenseName(_homeViewModel.EditExpenseName);
                    double newExpenseAmount = _formattingService.FormatExpenseAmount(_homeViewModel.EditExpenseAmount);

                    _databaseService.UpdateExpense
                    (
                        _homeViewModel.SelectedRow.Id,
                        _homeViewModel.SelectedCategory.Name,
                        _formattingService.FormatExpenseDate(newExpenseDate),
                        newExpenseName,
                        newExpenseAmount
                    );

                    foreach (var expense in _homeViewModel.ExpenseCollection)
                    {
                        if (expense.Id == _homeViewModel.SelectedRow.Id)
                        {
                            expense.Date = newExpenseDate;
                            expense.Name = newExpenseName;
                            expense.Amount = newExpenseAmount;
                        }
                    }

                    _homeViewModel.MessageBoxService.ShowMessageBox(MessageType.Information, "Výdaj byl úspěšně upraven.");

                    _homeViewModel.EditExpenseName = string.Empty;
                    _homeViewModel.EditExpenseAmount = string.Empty;

                    _homeViewModel.EditExpenseConfirmation = false;
                    _homeViewModel.EditExpenseModalVisible = false;
                }
                else
                {
                    _homeViewModel.EditExpenseDate = _homeViewModel.SelectedRow.Date;
                    _homeViewModel.EditExpenseName = _homeViewModel.SelectedRow.Name;
                    _homeViewModel.EditExpenseAmount = _homeViewModel.SelectedRow.Amount.ToString();

                    _homeViewModel.EditExpenseConfirmation = true;
                    _homeViewModel.EditExpenseModalVisible = true;
                }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_homeViewModel.EditExpenseConfirmation) ||
                e.PropertyName == nameof(_homeViewModel.EditExpenseDate) ||
                e.PropertyName == nameof(_homeViewModel.EditExpenseName) ||
                e.PropertyName == nameof(_homeViewModel.EditExpenseAmount))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
