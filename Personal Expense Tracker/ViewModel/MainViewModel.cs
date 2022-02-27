using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly FormattingService _formattingService;
        private readonly DatabaseService _databaseService;

        private readonly NavigationStore _navigationStore;

        public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(FormattingService formattingService, DatabaseService databaseService, NavigationStore navigationStore)
        {
            _formattingService = formattingService;
            _databaseService = databaseService;

            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentViewModel));
        }
    }
}
