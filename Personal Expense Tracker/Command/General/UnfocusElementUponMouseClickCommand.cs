using System;
using System.Windows;
using System.Windows.Input;

namespace Personal_Expense_Tracker.Command.General
{
    public class UnfocusElementUponMouseClickCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (!Keyboard.PrimaryDevice.IsKeyDown(Key.Tab))
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
}
