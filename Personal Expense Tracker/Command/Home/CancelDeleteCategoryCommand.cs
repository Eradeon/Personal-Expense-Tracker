using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class CancelDeleteCategoryCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public CancelDeleteCategoryCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.DeleteCategoryConfirmation = false;
        }
    }
}
