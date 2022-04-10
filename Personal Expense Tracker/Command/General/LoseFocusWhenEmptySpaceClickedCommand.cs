using System;
using System.Windows;

namespace Personal_Expense_Tracker.Command.General
{
    public class LoseFocusWhenEmptySpaceClickedCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter is UIElement)
            {
                UIElement uiElement = (UIElement)parameter;

                if (uiElement.Focusable)
                    uiElement.Focus();
            }
        }
    }
}
