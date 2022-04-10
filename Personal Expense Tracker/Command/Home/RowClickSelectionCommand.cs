using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    public class RowClickSelectionCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public RowClickSelectionCommand(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter is MouseButtonEventArgs)
            {
                MouseButtonEventArgs args = (MouseButtonEventArgs)parameter;

                if (args.OriginalSource != null)
                {
                    if (args.OriginalSource is Button)
                        return;

                    if (args.OriginalSource is DependencyObject)
                    {
                        DataGridRow? row = FindParent<DataGridRow>((DependencyObject)args.OriginalSource);

                        if (row != null)
                        {
                            var expense = (ExpenseViewModel)row.DataContext;

                            if (expense.IsSelected && !expense.IsEditing)
                            {
                                expense.IsSelected = false;
                                _homeViewModel.SelectedItemsCount--;
                            }
                            else if (!expense.IsSelected && !expense.IsEditing)
                            {
                                expense.IsSelected = true;
                                _homeViewModel.SelectedItemsCount++;
                            }
                        }
                    }
                }
            }
        }

        private T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T)
                return (T)parentObject;
            else
                return FindParent<T>(parentObject);
        }
    }
}
