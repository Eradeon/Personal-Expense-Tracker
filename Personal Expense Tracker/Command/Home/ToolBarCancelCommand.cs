using System;
using System.Windows.Controls;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class ToolBarCancelCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter is RadioButton)
            {
                ((RadioButton)parameter).IsChecked = true;
            }
        }
    }
}
