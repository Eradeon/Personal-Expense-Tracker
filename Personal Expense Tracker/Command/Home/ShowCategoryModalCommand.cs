using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class ShowCategoryModalCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

        public ShowCategoryModalCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.CategoryManagementModalVisible = true;
        }
    }
}
