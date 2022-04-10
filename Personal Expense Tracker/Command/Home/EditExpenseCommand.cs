using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Home
{
    public class EditExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        public EditExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;
            _messageBoxStore = messageBoxStore;

            _homeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_homeViewModel.EditExpenseName) &&
                 !string.IsNullOrWhiteSpace(_homeViewModel.EditExpenseAmount) &&
                 base.CanExecute(parameter);
                
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedExpense != null && _homeViewModel.SelectedCategory != null)
            {
                DateTime newExpenseDate = _homeViewModel.EditExpenseDate;
                string newExpenseName = _formattingService.FormatExpenseName(_homeViewModel.EditExpenseName);
                double newExpenseAmount = _formattingService.FormatExpenseAmount(_homeViewModel.EditExpenseAmount);

                var result = _databaseService.UpdateExpense
                (
                    _homeViewModel.SelectedExpense.Id,
                    _homeViewModel.SelectedCategory.Name,
                    _formattingService.FormatExpenseDate(newExpenseDate),
                    newExpenseName,
                    newExpenseAmount
                );

                if (result == DatabaseActionResult.Success)
                {
                    foreach (var expense in _homeViewModel.ExpenseCollection)
                    {
                        if (expense.Id == _homeViewModel.SelectedExpense.Id)
                        {
                            if ((_homeViewModel.GroupByMonth && newExpenseDate.Month == _homeViewModel.SelectedMonth + 1 && newExpenseDate.Year == _homeViewModel.SelectedYear) ||
                                (!_homeViewModel.GroupByMonth && newExpenseDate.Year == _homeViewModel.SelectedYear))
                            {
                                expense.Date = newExpenseDate;
                                expense.Name = newExpenseName;
                                expense.Amount = newExpenseAmount;
                                expense.IsEditing = false;
                                break;
                            }
                            else
                            {
                                _homeViewModel.ExpenseCollection.Remove(expense);
                                break;
                            }
                        }
                    }

                    _messageBoxStore.ShowMessageBox(MessageType.Information, "Výdaj byl úspěšně upraven.");

                    _homeViewModel.SelectedExpense = null;
                    _homeViewModel.EditExpenseName = string.Empty;
                    _homeViewModel.EditExpenseAmount = string.Empty;

                    _homeViewModel.ToolBarCancelCommand.Execute(null);
                }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_homeViewModel.EditExpenseDate) ||
                e.PropertyName == nameof(_homeViewModel.EditExpenseName) ||
                e.PropertyName == nameof(_homeViewModel.EditExpenseAmount))
            {
                OnCanExecuteChanged();
            }
        }

        public override void Dispose()
        {
            _homeViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            base.Dispose();
        }
    }
}
