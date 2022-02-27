using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class ShowCategoryModalCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public ShowCategoryModalCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.CategoryManagementModalVisible = true;
        }
    }
}
