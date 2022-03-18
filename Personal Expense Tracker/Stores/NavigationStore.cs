using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Stores
{
    internal class NavigationStore
    {
        public event Action? CurrentViewModelChanged;

        private BaseViewModel? _currentViewModel;
        public BaseViewModel? CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel?.Dispose();

                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
