using System;

namespace Personal_Expense_Tracker.Model
{
    internal class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public Expense(int id, DateTime date, string name, double amount)
        {
            Id = id; 
            Date = date;
            Name = name;
            Amount = amount;
        }
    }
}
