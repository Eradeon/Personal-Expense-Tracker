using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class HideCategoryModalCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

        public HideCategoryModalCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.CategoryManagementModalVisible = false;
            _mainViewModel.NewCategoryName = string.Empty;
            _mainViewModel.NewCategoryGroupByMonth = false;
            _mainViewModel.DeleteCategoryConfirmation = false;
            _mainViewModel.RenamedCategoryName = string.Empty;
        }
    }
}
