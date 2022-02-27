using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class DeleteExpenseCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;

        public DeleteExpenseCommand(HomeViewModel homeViewModel, DatabaseService databaseService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedCategory != null)
            {
                if (_homeViewModel.DeleteExpenseConfirmation)
                {
                    if (_homeViewModel.SelectedRow != null)
                    {
                        string date = _homeViewModel.SelectedRow.Date.ToString("dd.MM.yyyy");
                        string name = _homeViewModel.SelectedRow.Name.ToString();
                        string amount = _homeViewModel.SelectedRow.Amount.ToString("C2");

                        _databaseService.DeleteExpense
                        (
                            _homeViewModel.SelectedRow.Id,
                            _homeViewModel.SelectedCategory.Name
                        );

                        _homeViewModel.ExpenseCollection.Remove(_homeViewModel.SelectedRow);

                        _homeViewModel.MessageBoxService.ShowMessageBox(MessageType.Information, $"Výdaj {name} z {date} ve výši {amount} byl úspěšně odstraněn.");

                        _homeViewModel.DeleteExpenseConfirmation = false;
                        _homeViewModel.DeleteExpenseModalVisible = false;
                    }
                }
                else
                {
                    _homeViewModel.DeleteExpenseConfirmation = true;
                    _homeViewModel.DeleteExpenseModalVisible = true;
                }
            }
        }
    }
}
