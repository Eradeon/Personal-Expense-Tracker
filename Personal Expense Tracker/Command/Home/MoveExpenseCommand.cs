using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Home
{
    public class MoveExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        public MoveExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
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
                if (_homeViewModel.SelectedMoveToCategory != null)
                {
                    if (_homeViewModel.SelectedCategory != _homeViewModel.SelectedMoveToCategory)
                    {
                        List<ExpenseViewModel> itemsToMove = _homeViewModel.ExpenseCollection.Where(x => x.IsSelected == true).ToList();

                        if (itemsToMove.Count > 0)
                        {
                            var result = _databaseService.MoveRecords
                            (
                                _homeViewModel.SelectedCategory.Name,
                                _homeViewModel.SelectedMoveToCategory.Name,
                                itemsToMove
                            );

                            if (result == DatabaseActionResult.Success)
                            {
                                int item = 1;
                                _homeViewModel.LoadingExpenses = true;

                                foreach (var expense in itemsToMove)
                                {
                                    if (item == itemsToMove.Count)
                                        _homeViewModel.LoadingExpenses = false;

                                    _homeViewModel.ExpenseCollection.Remove(expense);

                                    item++;
                                }

                                _homeViewModel.SelectedItemsCount = 0;
                                _messageBoxStore.ShowMessageBox(MessageType.Information, $"Úspěšně přesunuto záznamů: {itemsToMove.Count}. Přesunuto do kategorie {_homeViewModel.SelectedMoveToCategory.DisplayName}.");
                            }
                        }
                        else
                            _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyly nalezeny žádné záznamy k přesunutí.");
                    }
                    else
                        _messageBoxStore.ShowMessageBox(MessageType.Warning, "Cílová kategorie musí být odlišná od zdrojové kategorie.");
                }
                else
                    _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyla zvolena cílová kategorie.");
            }
            else
                _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nebyly nalezeny žádné záznamy k přesunutí.");
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HomeViewModel.SelectedItemsCount))
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
