using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Stores
{
    internal enum NavigationType
    {
        Home
    }

    internal class NavigationStore
    {
        public event Action? CurrentViewModelChanged;

        private BaseViewModel? _currentViewModel;
        public BaseViewModel? CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnCurrentViewModelChanged(); }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
