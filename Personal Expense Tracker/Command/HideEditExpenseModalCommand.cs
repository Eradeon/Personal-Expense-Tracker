using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class HideEditExpenseModalCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

        public HideEditExpenseModalCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.EditExpenseName = string.Empty;
            _mainViewModel.EditExpenseAmount = string.Empty;
            _mainViewModel.EditExpenseConfirmation = false;
            _mainViewModel.EditExpenseModalVisible = false;
        }
    }
}
