using System;
using System.Globalization;
using System.Windows.Data;

namespace Personal_Expense_Tracker.Service
{
    internal class StringEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == string.Empty || (string)value == null ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
