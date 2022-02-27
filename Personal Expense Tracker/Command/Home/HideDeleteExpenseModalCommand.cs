using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class HideDeleteExpenseModalCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public HideDeleteExpenseModalCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.DeleteExpenseConfirmation = false;
            _homeViewModel.DeleteExpenseModalVisible = false;
        }
    }
}
