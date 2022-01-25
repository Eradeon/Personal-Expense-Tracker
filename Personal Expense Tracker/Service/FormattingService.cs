using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Personal_Expense_Tracker.Service
{
    internal class FormattingService
    {
        public string FormatCategoryTableName(string name)
        {
            name = name.Normalize(System.Text.NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(name[i]) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(name[i]);
                }
            }

            name = sb.ToString();

            name = Regex.Replace(name, "[^a-zA-Z0-9 -]", "").ToLower();

            return Regex.Replace(name.Trim(), @"\s+", "_") + "_expenses";
        }

        public string FormatCategoryDisplayName(string displayName)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(displayName.Trim().ToLower(), @"\s+", " "));
        }

        public string FormatExpenseDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        
        public string FormatExpenseName(string expenseName)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(expenseName.Trim().ToLower(), @"\s+", " "));
        }

        public double FormatExpenseAmount(string expenseAmount)
        {
            return Math.Round(double.Parse(expenseAmount.Trim()), 2, MidpointRounding.AwayFromZero);
        }
    }
}
