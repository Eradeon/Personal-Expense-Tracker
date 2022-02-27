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

            FormattingService _formattingService = new FormattingService();
            DatabaseService _databaseService = new DatabaseService(_formattingService);

            _databaseService.CreateDefaultDatabase();

            NavigationStore _navigationStore = new NavigationStore();
            _navigationStore.CurrentViewModel = new HomeViewModel(_databaseService, _formattingService);

            Window mainWindow = new MainWindow
            (
                new MainViewModel(_formattingService, _databaseService, _navigationStore)
            );
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
