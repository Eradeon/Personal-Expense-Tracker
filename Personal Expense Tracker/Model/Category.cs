using System;

namespace Personal_Expense_Tracker.Model
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool GroupByMonth { get; set; }

        public Category(int id, string name, string displayName, bool groupByMonth)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
            GroupByMonth = groupByMonth;
        }
    }
}
