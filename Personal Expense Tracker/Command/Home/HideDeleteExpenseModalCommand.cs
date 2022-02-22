using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class HideDeleteExpenseModalCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

        public HideDeleteExpenseModalCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.DeleteExpenseConfirmation = false;
            _mainViewModel.DeleteExpenseModalVisible = false;
        }
    }
}
