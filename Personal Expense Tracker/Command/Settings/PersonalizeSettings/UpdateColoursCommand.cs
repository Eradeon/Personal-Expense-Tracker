using System;
using System.Collections.ObjectModel;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Settings.PersonalizeSettings
{
    public class UpdateColoursCommand : BaseCommand
    {
        private readonly ThemeStore _themeStore;
        private readonly MessageBoxStore _messageBoxStore;
        private readonly ConfigurationService _configurationService;
        private readonly ThemeType _themeType;
        private readonly ObservableCollection<ThemeColourViewModel> _colours;

        public UpdateColoursCommand(ThemeStore themeStore, MessageBoxStore messageBoxStore, ConfigurationService configurationService, ThemeType themeType, ObservableCollection<ThemeColourViewModel> colours)
        {
            _themeStore = themeStore;
            _messageBoxStore = messageBoxStore;
            _configurationService = configurationService;
            _themeType = themeType;
            _colours = colours;
        }

        public override void Execute(object? parameter)
        {
            foreach (var colour in _colours)
            {
                if (!colour.IsColourValid)
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Warning, "Jedno nebo více polí neobsahuje platný hexadecimální kód.");
                    return;
                }
            }

            _themeStore.ClearTheme(_themeType);

            foreach (var colour in _colours)
            {
                _configurationService.UpdateAppThemeColour(_themeType, colour.Key, null, null, colour.Colour);
                _themeStore.AddColourToTheme(_themeType, colour.Name, colour.Colour);
            }

            if (_themeStore.DarkModeEnabled && _themeType == ThemeType.Dark)
                _themeStore.SwitchToTheme(_themeType);
            else if (!_themeStore.DarkModeEnabled && _themeType == ThemeType.Light)
                _themeStore.SwitchToTheme(_themeType);

            _messageBoxStore.ShowMessageBox(MessageType.Information, "Barevné schéma bylo úspěšně aktualizováno.");
        }
    }
}
