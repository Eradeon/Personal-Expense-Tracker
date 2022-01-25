using System;
using Personal_Expense_Tracker.Model;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class CategoryViewModel : BaseViewModel
    {
        private readonly Category _category;

        public int Id
        {
            get { return _category.Id; }
            set { _category.Id = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return _category.Name; }
            set { _category.Name = value; RaisePropertyChanged(); }
        }

        public string DisplayName
        {
            get { return _category.DisplayName; }
            set { _category.DisplayName = value; RaisePropertyChanged(); }
        }

        public bool GroupByMonth
        {
            get { return _category.GroupByMonth; }
            set { _category.GroupByMonth = value; RaisePropertyChanged(); }
        }

        public CategoryViewModel(Category category)
        {
            _category = category;
        }
    }
}
