﻿using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    public class SelectAllCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public SelectAllCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.ExpenseCollection != null && _homeViewModel.ExpenseCollection.Count > 0)
            {
                foreach (var item in _homeViewModel.ExpenseCollection)
                {
                    item.IsSelected = true;
                    item.IsEditing = false;
                }

                _homeViewModel.SelectedItemsCount = _homeViewModel.ExpenseCollection.Count;

                if (_homeViewModel.SelectedToolBar == ExpenseToolBar.Edit)
                {
                    _homeViewModel.SelectedExpense = null;
                    _homeViewModel.CancelToolBarSelection = true;
                    _homeViewModel.SelectedToolBar = ExpenseToolBar.None;
                }
            }
        }
    }
}
