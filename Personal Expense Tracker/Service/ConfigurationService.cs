using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Service
{
    internal class ConfigurationService
    {
        public bool GetBoolFromConfig(string key)
        {
            return bool.TryParse(ConfigurationManager.AppSettings[key], out bool result) ? result : false;
        }

        public string GetStringFromConfig(string key)
        {
            return string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[key]) ? string.Empty : ConfigurationManager.AppSettings[key]!;
        }

        public List<ThemeColourElement> GetThemeColours(string themeName)
        {
            var section = (ThemeColoursSection)ConfigurationManager.GetSection("appThemes/" + themeName);

            return (from object colour in section.ThemeColours select ((ThemeColourElement)colour)).ToList();
        }

        public ThemeColourElement GetSpecificThemeColour(string themeName, string key)
        {
            var section = (ThemeColoursSection)ConfigurationManager.GetSection("appThemes/" + themeName);

            return section.ThemeColours[key];
        }

        public void UpdateAppSettings(string key, string parameter)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = parameter;

            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void UpdateAppThemes(ThemeType themeType, string colourKey, string? colourName, string? colourDisplayName, string? colourCode)
        {
            string themeName = string.Empty;

            if (themeType == ThemeType.Light)
                themeName = "lightTheme";
            else if (themeType == ThemeType.Dark)
                themeName = "darkTheme";

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = (ThemeColoursSection)configuration.GetSection("appThemes/" + themeName);
            
            if (colourName != null)
                section.ThemeColours[colourKey].Name = colourName;
            
            if (colourDisplayName != null)
                section.ThemeColours[colourKey].DisplayName = colourDisplayName;
            
            if (colourCode != null)
                section.ThemeColours[colourKey].ColourCode = colourCode;

            configuration.Save();
            ConfigurationManager.RefreshSection("appThemes/lightTheme");
        }
    }
}
