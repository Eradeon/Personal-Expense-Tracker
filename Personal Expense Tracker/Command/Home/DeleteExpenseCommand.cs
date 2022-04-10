using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Stores;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Personal_Expense_Tracker.Command.Home
{
    public class DeleteExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly MessageBoxStore _messageBoxStore;

        public DeleteExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService, MessageBoxStore messageBoxStore)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _messageBoxStore = messageBoxStore;

            _homeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return _homeViewModel.SelectedItemsCount > 0 &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.ExpenseCollection != null && _homeViewModel.ExpenseCollection.Count > 0 && _homeViewModel.SelectedCategory != null)
            {
                List<ExpenseViewModel> itemsToRemove = _homeViewModel.ExpenseCollection.Where(x => x.IsSelected == true).ToList();

                if (itemsToRemove.Count > 0)
                {
                    DatabaseActionResult result = _databaseService.DeleteExpense(_homeViewModel.SelectedCategory.Name, itemsToRemove);

                    if (result == DatabaseActionResult.Success)
                    {
                        int item = 1;
                        _homeViewModel.LoadingExpenses = true;

                        foreach (var expense in itemsToRemove)
                        {
                            if (item == itemsToRemove.Count)
                                _homeViewModel.LoadingExpenses = false;

                            _homeViewModel.ExpenseCollection.Remove(expense);

                            item++;
                        }

                        _homeViewModel.SelectedItemsCount = 0;
                        _messageBoxStore.ShowMessageBox(MessageType.Information, $"Úspěšně odstraněno záznamů: {itemsToRemove.Count}");
                    }
                }
                else
                    _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyly nalezeny žádné záznamy k odstranění.");
            }
            else
                _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyly nalezeny žádné záznamy k odstranění.");
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_homeViewModel.SelectedItemsCount))
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
