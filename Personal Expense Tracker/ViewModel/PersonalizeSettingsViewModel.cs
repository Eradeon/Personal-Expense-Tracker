using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Command.General;
using Personal_Expense_Tracker.Command.Settings.PersonalizeSettings;

namespace Personal_Expense_Tracker.ViewModel
{
    public class PersonalizeSettingsViewModel : BaseViewModel
    {
        #region Services & Stores
        private readonly ConfigurationService _configurationService;
        #endregion Services & Stores

        #region Collections
        private ObservableCollection<ThemeColourViewModel> _lightThemeColours;
        public ObservableCollection<ThemeColourViewModel> LightThemeColours
        {
            get { return _lightThemeColours; }
            set { _lightThemeColours = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ThemeColourViewModel> _darkThemeColours;
        public ObservableCollection<ThemeColourViewModel> DarkThemeColours
        {
            get { return _darkThemeColours; }
            set { _darkThemeColours = value; RaisePropertyChanged(); }
        }
        #endregion Collections

        #region Commands
        public ICommand UpdateLightThemeColoursCommand { get; }
        public ICommand UpdateDarkThemeColoursCommand { get; }
        public ICommand UnfocusElementUponMouseClickCommand { get; }
        #endregion Commands

        public PersonalizeSettingsViewModel(ConfigurationService configurationService, ThemeStore themeStore, MessageBoxStore messageBoxStore)
        {
            _configurationService = configurationService;

            _lightThemeColours = LoadTheme("lightTheme");
            _darkThemeColours = LoadTheme("darkTheme");

            //Commands
            UpdateLightThemeColoursCommand = new UpdateColoursCommand(themeStore, messageBoxStore, configurationService, ThemeType.Light, LightThemeColours);
            UpdateDarkThemeColoursCommand = new UpdateColoursCommand(themeStore, messageBoxStore, configurationService, ThemeType.Dark, DarkThemeColours);
            UnfocusElementUponMouseClickCommand = new UnfocusElementUponMouseClickCommand();
        }

        private ObservableCollection<ThemeColourViewModel> LoadTheme(string themeName)
        {
            ObservableCollection<ThemeColourViewModel> collection = new ObservableCollection<ThemeColourViewModel>();

            List<ThemeColourElement> colourList = _configurationService.GetThemeColours(themeName);

            if (colourList != null && colourList.Count > 0)
            {
                foreach (ThemeColourElement colour in colourList)
                {
                    collection.Add(new ThemeColourViewModel(new ThemeColour
                    (
                        colour.Key,
                        colour.Name,
                        colour.DisplayName,
                        colour.ColourCode,
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString(colour.ColourCode)),
                        true
                    )));
                }
            }

            return collection;
        }
    }
}
