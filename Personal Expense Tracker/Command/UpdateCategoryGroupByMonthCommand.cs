using System;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;

namespace Personal_Expense_Tracker.Command
{
    internal class UpdateCategoryGroupByMonthCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;

        public UpdateCategoryGroupByMonthCommand(MainViewModel mainViewModel, DatabaseService databaseService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            _databaseService.UpdateGroupByMonth
            (
                _mainViewModel.GetSelectedCategoryTableId(false),
                _mainViewModel.GroupByMonth
            );

            _mainViewModel.ChangeCategoryGroupByMonth(_mainViewModel.GroupByMonth);
        }
    }
}
