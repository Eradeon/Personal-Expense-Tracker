using System;
using System.Windows.Input;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Command.General;
using Personal_Expense_Tracker.Command.Main;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        #region Services & Stores
        private readonly NavigationStore _navigationStore;
        private readonly MessageBoxStore _messageBoxStore;
        #endregion Services & Stores

        public BaseViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;

        private bool _darkModeEnabled;
        public bool DarkModeEnabled
        {
            get { return _darkModeEnabled; }
            set { _darkModeEnabled = value; }
        }

        #region MessageBox Properties
        public bool MessageBoxVisible => _messageBoxStore.MessageBoxVisible;
        public bool IsMessageBoxClosing => _messageBoxStore.IsMessageBoxClosing;
        public MessageType MessageBoxType => _messageBoxStore.MessageBoxType;
        public string MessageBoxText => _messageBoxStore.MessageBoxText;
        #endregion MessageBox Properties

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateYearlyStatisticsCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        public ICommand ToggleDarkModeCommand { get; }
        public ICommand CloseMessageBoxCommand { get; }

        public ICommand UnfocusElementUponMouseClickCommand { get; }
        public ICommand LoseFocusWhenEmptySpaceClickedCommand { get; }
        #endregion Commands

        public MainViewModel(FormattingService formattingService, DatabaseService databaseService, ConfigurationService configurationService,
                             NavigationStore navigationStore, MessageBoxStore messageBoxStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _messageBoxStore = messageBoxStore;
            _messageBoxStore.MessageBoxUpdated += OnMessageBoxUpdated;

            _darkModeEnabled = configurationService.GetBoolFromConfig("dark_mode");

            //Commands
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(databaseService, formattingService, messageBoxStore));
            NavigateYearlyStatisticsCommand = new NavigateCommand<YearlyStatisticsViewModel>(navigationStore, () => new YearlyStatisticsViewModel());
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(messageBoxStore, formattingService, databaseService, configurationService));

            ToggleDarkModeCommand = new ToggleDarkModeCommand(this, configurationService);
            CloseMessageBoxCommand = new CloseMessageBoxCommand(messageBoxStore);

            UnfocusElementUponMouseClickCommand = new UnfocusElementUponMouseClickCommand();
            LoseFocusWhenEmptySpaceClickedCommand = new LoseFocusWhenEmptySpaceClickedCommand();
        }

        private void OnCurrentViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentViewModel));
        }

        private void OnMessageBoxUpdated()
        {
            RaisePropertyChanged(nameof(MessageBoxVisible));
            RaisePropertyChanged(nameof(IsMessageBoxClosing));
            RaisePropertyChanged(nameof(MessageBoxType));
            RaisePropertyChanged(nameof(MessageBoxText));
        }
    }
}
