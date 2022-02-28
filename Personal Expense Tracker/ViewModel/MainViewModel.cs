using System;
using System.Windows.Input;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Command.General;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        #region Services & Stores
        private readonly NavigationStore _navigationStore;
        #endregion Services & Stores

        public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateYearlyStatisticsCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        #endregion Commands

        public MainViewModel(FormattingService formattingService, DatabaseService databaseService, NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            //Commands
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(databaseService, formattingService));
            NavigateYearlyStatisticsCommand = new NavigateCommand<YearlyStatisticsViewModel>(navigationStore, () => new YearlyStatisticsViewModel());
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel());
        }

        private void OnCurrentViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentViewModel));
        }
    }
}
