using System;
using System.Windows.Controls;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class BeginExpenseEditCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public BeginExpenseEditCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter is DataGridRow)
            {
                var row = (DataGridRow)parameter;

                if (row != null)
                {
                    var expense = (ExpenseViewModel)row.DataContext;

                    if (expense != null)
                    {
                        _homeViewModel.DeselectAllCommand.Execute(null);
                        
                        if (expense.IsSelected)
                        {
                            expense.IsSelected = false;
                            _homeViewModel.SelectedItemsCount--;
                        }

                        _homeViewModel.SelectedExpense = expense;
                        _homeViewModel.EditExpenseDate = expense.Date;
                        _homeViewModel.EditExpenseName = expense.Name;
                        _homeViewModel.EditExpenseAmount = expense.Amount.ToString();
                        expense.IsEditing = true;
                        _homeViewModel.SelectedToolBar = ExpenseToolBar.Edit;
                        _homeViewModel.CancelToolBarSelection = true;
                    }
                }
            }
        }
    }
}
