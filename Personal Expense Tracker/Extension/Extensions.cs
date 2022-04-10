using System;

namespace Personal_Expense_Tracker.Extension
{
    public static class Extensions
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
