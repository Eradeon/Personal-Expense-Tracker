using System;
using System.Reflection;

namespace Personal_Expense_Tracker.ViewModel
{
    public class AboutSettingsViewModel : BaseViewModel
    {
        private string _appVersion = string.Empty;
        public string AppVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; RaisePropertyChanged(); }
        }

        private string _appVersionType = string.Empty;
        public string AppVersionType
        {
            get { return _appVersionType; }
            set { _appVersionType = value; RaisePropertyChanged(); }
        }

        public AboutSettingsViewModel()
        {
            var version = typeof(AboutSettingsViewModel).Assembly.GetName().Version;
            if (version != null)
                _appVersion = version.ToString();
            else
                _appVersion = "N/A";

            var versionType = typeof(AboutSettingsViewModel).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (versionType != null)
                _appVersionType = versionType.InformationalVersion.ToString();
            else
                _appVersionType = "N/A";
        }
    }
}
