using System;
using Personal_Expense_Tracker.Model;

namespace Personal_Expense_Tracker.ViewModel
{
    public class ExpenseViewModel : BaseViewModel
    {
        private readonly Expense _expense;

        public int Id
        {
            get { return _expense.Id; }
            set { _expense.Id = value; RaisePropertyChanged(); }
        }

        public DateTime Date
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

        public bool IsSelected
        {
            get { return _expense.IsSelected; }
            set { _expense.IsSelected = value; RaisePropertyChanged(); }
        }

        public bool IsEditing
        {
            get { return _expense.IsEditing; }
            set { _expense.IsEditing = value; RaisePropertyChanged(); }
        }

        public ExpenseViewModel(Expense expense)
        {
            _expense = expense;
        }
    }
}
