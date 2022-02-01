using System;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;

namespace Personal_Expense_Tracker.Command
{
    internal class CategoryChangedCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

        public CategoryChangedCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.GroupByMonth = _mainViewModel.GetSelectedCategoryGroupByMonth();
        }
    }
}
