using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class HideEditExpenseModalCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public HideEditExpenseModalCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.EditExpenseName = string.Empty;
            _homeViewModel.EditExpenseAmount = string.Empty;
            _homeViewModel.EditExpenseConfirmation = false;
            _homeViewModel.EditExpenseModalVisible = false;
        }
    }
}
