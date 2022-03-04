using System;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;

namespace Personal_Expense_Tracker.Command.Main
{
    internal class ToggleDarkModeCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ConfigurationService _configurationService;

        public ToggleDarkModeCommand(MainViewModel mainViewModel, ConfigurationService configurationService)
        {
            _mainViewModel = mainViewModel;
            _configurationService = configurationService;
        }

        public override void Execute(object? parameter)
        {
            _configurationService.UpdateConfiguration("dark_mode", _mainViewModel.DarkModeEnabled.ToString());
        }
    }
}
