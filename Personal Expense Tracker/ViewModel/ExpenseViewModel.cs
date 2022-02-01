using System;
using Personal_Expense_Tracker.Model;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class ExpenseViewModel : BaseViewModel
    {
        private readonly Expense _expense;

        public int Id
        {
            get { return _expense.Id; }
            set { _expense.Id = value; RaisePropertyChanged(); }
        }

        public string Date
        {
            get { return _expense.Date; }
            set { _expense.Date = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return _expense.Name; }
            set { _expense.Name = value; RaisePropertyChanged(); }
        }

        public double Amount
        {
            get { return _expense.Amount; }
            set { _expense.Amount = value; RaisePropertyChanged(); }
        }

        public ExpenseViewModel(Expense expense)
        {
            _expense = expense;
        }
    }
}
