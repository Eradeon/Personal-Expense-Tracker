using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Stores
{
    internal enum ThemeType
    {
        Light,
        Dark
    }

    internal class ThemeStore
    {
        #region Services & Stores
        private readonly ConfigurationService _configurationService;
        private readonly MessageBoxStore _messageBoxStore;
        #endregion Services & Stores

        private bool _darkModeEnabled;
        public bool DarkModeEnabled
        {
            get { return _darkModeEnabled; }
            set { _darkModeEnabled = value; }
        }

        #region Themes
        private ResourceDictionary _lightTheme;
        public ResourceDictionary LightTheme
        {
            get { return _lightTheme; }
            set { _lightTheme = value; }
        }

        private ResourceDictionary _darkTheme;
        public ResourceDictionary DarkTheme
        {
            get { return _darkTheme; }
            set { _darkTheme = value; }
        }
        #endregion Themes

        public ThemeStore(ConfigurationService configurationService, MessageBoxStore messageBoxStore)
        {
            _configurationService = configurationService;
            _messageBoxStore = messageBoxStore;

            _darkModeEnabled = configurationService.GetBoolFromConfig("dark_mode");

            _lightTheme = LoadTheme("lightTheme");
            _darkTheme= LoadTheme("darkTheme");

            if (_darkModeEnabled)
                SwitchToTheme(ThemeType.Dark);
            else
                SwitchToTheme(ThemeType.Light);
        }

        public void SwitchToTheme(ThemeType theme)
        {
            ResourceDictionary? coloursResource = null;
            ResourceDictionary? brushesResource = null;

            foreach (var item in Application.Current.Resources.MergedDictionaries)
            {
                if (item.Source.OriginalString == "/Style/Colours.xaml")
                    coloursResource = item;

                if (item.Source.OriginalString == "/Style/ColourBrushes.xaml")
                    brushesResource = item;

                if (coloursResource != null && brushesResource != null)
                    break;
            }

            if (coloursResource != null && brushesResource != null)
            {
                if (theme == ThemeType.Dark)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(coloursResource);
                    Application.Current.Resources.MergedDictionaries.Add(_darkTheme);
                }
                else if (theme == ThemeType.Light)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(coloursResource);
                    Application.Current.Resources.MergedDictionaries.Add(_lightTheme);
                }

                Application.Current.Resources.MergedDictionaries.Remove(brushesResource);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/Style/ColourBrushes.xaml", UriKind.Relative)});
            }
            else
            {
                _messageBoxStore.ShowMessageBox(MessageType.Error, "Chyba: Nelze změnit barevné schéma.");
            }
        }

        public void ClearTheme(ThemeType themeType)
        {
            if (themeType == ThemeType.Dark)
                DarkTheme.Clear();
            if (themeType == ThemeType.Light)
                LightTheme.Clear();
        }

        public void AddColourToTheme(ThemeType themeType, string colourName, string colourCode)
        {
            if (themeType == ThemeType.Dark)
                DarkTheme.Add(colourName, (Color)ColorConverter.ConvertFromString(colourCode));
            if (themeType == ThemeType.Light)
                LightTheme.Add(colourName, (Color)ColorConverter.ConvertFromString(colourCode));
        }

        private ResourceDictionary LoadTheme(string themeName)
        {
            ResourceDictionary theme = new ResourceDictionary() { Source = new Uri("/Style/Colours.xaml", UriKind.Relative) };

            var colourList = _configurationService.GetThemeColours(themeName);

            foreach (ThemeColourElement colour in colourList)
            {
                theme.Add(colour.Name, (Color)ColorConverter.ConvertFromString(colour.ColourCode));
            }

            return theme;
        }
    }
}
