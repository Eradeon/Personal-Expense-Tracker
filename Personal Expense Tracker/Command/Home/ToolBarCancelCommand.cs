using System;
using System.Windows.Controls;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class ToolBarCancelCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public ToolBarCancelCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.SelectedToolBar = ExpenseToolBar.None;
            _homeViewModel.CancelToolBarSelection = true;

            if (_homeViewModel.SelectedExpense != null)
            {
                foreach (var item in _homeViewModel.ExpenseCollection)
                {
                    if (item.IsEditing)
                    {
                        item.IsEditing = false;
                        break;
                    }
                }
                _homeViewModel.SelectedExpense = null;
            }
        }
    }
}
