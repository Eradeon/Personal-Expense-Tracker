using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class HideCategoryModalCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public HideCategoryModalCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.CategoryManagementModalVisible = false;
            _homeViewModel.NewCategoryName = string.Empty;
            _homeViewModel.NewCategoryGroupByMonth = false;
            _homeViewModel.DeleteCategoryConfirmation = false;
            _homeViewModel.RenamedCategoryName = string.Empty;
        }
    }
}
