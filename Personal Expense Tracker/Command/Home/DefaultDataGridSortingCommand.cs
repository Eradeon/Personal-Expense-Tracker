using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Personal_Expense_Tracker.Command.Home
{
    public class DefaultDataGridSortingCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                DataGrid dataGrid = (DataGrid)parameter;

                var column = dataGrid.Columns[0];

                dataGrid.Items.SortDescriptions.Clear();

                dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                foreach (DataGridColumn c in dataGrid.Columns)
                {
                    c.SortDirection = null;
                }

                column.SortDirection = ListSortDirection.Ascending;

                dataGrid.Items.Refresh();
            }
        }
    }
}
