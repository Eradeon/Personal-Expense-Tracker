using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Personal_Expense_Tracker.Command.General
{
    internal class UnfocusElementUponMouseClickCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                if (!Keyboard.PrimaryDevice.IsKeyDown(Key.Tab))
                {
                    ((Grid)parameter).Focus();
                }
            }
        }
    }
}
