using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    public class CategoryChangedCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public CategoryChangedCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedCategory != null)
            {
                _homeViewModel.GroupByMonth = _homeViewModel.SelectedCategory.GroupByMonth;
                _homeViewModel.LoadYears();
            }
        }
    }
}
