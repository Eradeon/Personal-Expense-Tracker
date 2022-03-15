using System;

namespace Personal_Expense_Tracker.Model
{
    internal class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IsSelected { get; set; }
        public bool IsEditing { get; set; }

        public Expense(int id, DateTime date, string name, double amount, bool isSelected, bool isEditing)
        {
            Id = id; 
            Date = date;
            Name = name;
            Amount = amount;
            IsSelected = isSelected;
            IsEditing = isEditing;
        }
    }
}
