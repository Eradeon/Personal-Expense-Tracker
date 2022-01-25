using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Expense_Tracker.Extension
{
    internal static class Extensions
    {
        public static int ToInt(this bool value) {
            return value ? 1 : 0;
        }

        public static bool ToBool(this int value)
        {
            return value != 0;
        }
    }
}
