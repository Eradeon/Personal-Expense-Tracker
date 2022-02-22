using System;
using System.Windows.Controls;

namespace Personal_Expense_Tracker.Command.General
{
    internal class LoseFocusWhenEmptySpaceClickedCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter is Grid)
            {
                ((Grid)parameter).Focus();
            }
        }
    }
}
