﻿using System;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class UpdateCategoryGroupByMonthCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;

        public UpdateCategoryGroupByMonthCommand(HomeViewModel homeViewModel, DatabaseService databaseService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.SelectedCategory != null)
            {
                _databaseService.UpdateGroupByMonth
            (
                _homeViewModel.SelectedCategory.Id,
                _homeViewModel.GroupByMonth
            );

                _homeViewModel.SelectedCategory.GroupByMonth = _homeViewModel.GroupByMonth;
            }
        }
    }
}
