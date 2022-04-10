using System;
using System.Windows.Input;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Command.General;

namespace Personal_Expense_Tracker.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Services & Stores
        public NavigationStore SettingsNavigationStore { get; }
        #endregion Services & Stores

        public BaseViewModel? CurrentSettingsViewModel => SettingsNavigationStore.CurrentViewModel;

        #region Commands
        public ICommand NavigateCategorySettingsCommand { get; }
        public ICommand NavigatePersonalizeSettingsCommand { get; }
        public ICommand NavigateAboutSettingsCommand { get; }

        public ICommand UnfocusElementUponMouseClickCommand { get; }
        #endregion Commands

        public SettingsViewModel(MessageBoxStore messageBoxStore, ThemeStore themeStore, FormattingService formattingService, DatabaseService databaseService, ConfigurationService configurationService)
        {
            SettingsNavigationStore = new NavigationStore();
            SettingsNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            //Commands
            NavigateCategorySettingsCommand = new NavigateCommand<CategorySettingsViewModel>(SettingsNavigationStore, () => new CategorySettingsViewModel(messageBoxStore, formattingService, databaseService));
            NavigatePersonalizeSettingsCommand = new NavigateCommand<PersonalizeSettingsViewModel>(SettingsNavigationStore, () => new PersonalizeSettingsViewModel(configurationService, themeStore, messageBoxStore));
            NavigateAboutSettingsCommand = new NavigateCommand<AboutSettingsViewModel>(SettingsNavigationStore, () => new AboutSettingsViewModel());

            UnfocusElementUponMouseClickCommand = new UnfocusElementUponMouseClickCommand();

            NavigateCategorySettingsCommand.Execute(null);
        }

        private void OnCurrentViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentSettingsViewModel));
        }

        public override void Dispose()
        {
            SettingsNavigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;

            base.Dispose();
        }
    }
}
