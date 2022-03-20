using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class DeselectAllCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public DeselectAllCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.ExpenseCollection != null && _homeViewModel.ExpenseCollection.Count > 0)
            {
                foreach (var item in _homeViewModel.ExpenseCollection)
                {
                    item.IsSelected = false;
                }

                _homeViewModel.SelectedItemsCount = 0;
            }
        }
    }
}
