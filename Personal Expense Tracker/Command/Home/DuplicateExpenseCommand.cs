using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Home
{
    public class DuplicateExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        public DuplicateExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;
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
                List<ExpenseViewModel> itemsToDuplicate = _homeViewModel.ExpenseCollection.Where(x => x.IsSelected == true).ToList();

                if (itemsToDuplicate.Count > 0)
                {
                    var result = _databaseService.DuplicateExpenses(_homeViewModel.SelectedCategory.Name, itemsToDuplicate);

                    if (result != null)
                    {
                        int item = 1;
                        _homeViewModel.LoadingExpenses = true;

                        foreach (var expense in result)
                        {
                            if (item == itemsToDuplicate.Count)
                                _homeViewModel.LoadingExpenses = false;

                            _homeViewModel.ExpenseCollection.Add(expense);

                            item++;
                        }

                        _messageBoxStore.ShowMessageBox(MessageType.Information, $"Úspěšně zduplikováno záznamů: {result.Count}");
                    }
                }
                else
                    _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyly nalezeny žádné záznamy k duplikování.");
            }
            else
                _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyly nalezeny žádné záznamy k duplikování.");
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HomeViewModel.SelectedItemsCount))
                OnCanExecuteChanged();
        }

        public override void Dispose()
        {
            _homeViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            base.Dispose();
        }
    }
}
