using System;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Main
{
    internal class ToggleDarkModeCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ConfigurationService _configurationService;
        private readonly ThemeStore _themeStore;

        public ToggleDarkModeCommand(MainViewModel mainViewModel, ConfigurationService configurationService, ThemeStore themeStore)
        {
            _mainViewModel = mainViewModel;
            _configurationService = configurationService;
            _themeStore = themeStore;
        }

        public override void Execute(object? parameter)
        {
            _configurationService.UpdateAppSettings("dark_mode", _mainViewModel.DarkModeEnabled.ToString());

            if (_mainViewModel.DarkModeEnabled)
                _themeStore.SwitchToTheme(ThemeType.Dark);
            else
                _themeStore.SwitchToTheme(ThemeType.Light);
        }
    }
}
