using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class ChangeToolBarCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly ExpenseToolBar _toolbar;

        public ChangeToolBarCommand(HomeViewModel homeViewModel, ExpenseToolBar toolbar)
        {
            _homeViewModel = homeViewModel;
            _toolbar = toolbar;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.SelectedToolBar = _toolbar;

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
