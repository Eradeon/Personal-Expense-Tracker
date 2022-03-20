using System;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;
using Personal_Expense_Tracker.View;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Set XAML culture to current culture
            FrameworkElement.LanguageProperty.OverrideMetadata
            (
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag))
            );

            FormattingService formattingService = new FormattingService();
            MessageBoxStore messageBoxStore = new MessageBoxStore();
            DatabaseService databaseService = new DatabaseService(formattingService, messageBoxStore);
            ConfigurationService configurationService = new ConfigurationService();

            databaseService.CreateDefaultDatabase();

            NavigationStore navigationStore = new NavigationStore();
            ThemeStore themeStore = new ThemeStore(configurationService, messageBoxStore);
            navigationStore.CurrentViewModel = new HomeViewModel(databaseService, formattingService, messageBoxStore);

            Window mainWindow = new MainWindow
            (
                new MainViewModel(formattingService, databaseService, configurationService,
                                  navigationStore, themeStore, messageBoxStore)
            );
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
