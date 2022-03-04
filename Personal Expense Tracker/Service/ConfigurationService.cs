using System;
using System.Configuration;

namespace Personal_Expense_Tracker.Service
{
    internal class ConfigurationService
    {
        public bool GetBoolFromConfig(string key)
        {
            return bool.TryParse(ConfigurationManager.AppSettings[key], out bool result) ? result : false;
        }

        public void UpdateConfiguration(string key, string parameter)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = parameter;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
