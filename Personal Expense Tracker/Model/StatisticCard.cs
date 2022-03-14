using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Expense_Tracker.Model
{
    internal class StatisticCard
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string SupportText { get; set; }
        public string TooltipText { get; set; }

        public StatisticCard(string icon, string title, string value, string supportText, string tooltipText)
        {
            Icon = icon;
            Title = title;
            Value = value;
            SupportText = supportText;
            TooltipText = tooltipText;
        }
    }
}
