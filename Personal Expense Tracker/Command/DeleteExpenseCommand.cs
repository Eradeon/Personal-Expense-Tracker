using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class DeleteExpenseCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;

        public DeleteExpenseCommand(MainViewModel mainViewModel, DatabaseService databaseService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_mainViewModel.SelectedCategory != null)
            {
                if (_mainViewModel.DeleteExpenseConfirmation)
                {
                    if (_mainViewModel.SelectedRow != null)
                    {
                        _databaseService.DeleteExpense
                        (
                            _mainViewModel.SelectedRow.Id,
                            _mainViewModel.SelectedCategory.Name
                        );

                        _mainViewModel.ExpenseCollection.Remove(_mainViewModel.SelectedRow);

                        _mainViewModel.DeleteExpenseConfirmation = false;
                        _mainViewModel.DeleteExpenseModalVisible = false;
                    }
                }
                else
                {
                    _mainViewModel.DeleteExpenseConfirmation = true;
                    _mainViewModel.DeleteExpenseModalVisible = true;
                }
            }
        }
    }
}
