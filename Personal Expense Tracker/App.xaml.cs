using System;
using System.Windows;
using Personal_Expense_Tracker.View;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using System.Windows.Markup;
using System.Globalization;

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

            DatabaseService _databaseService = new DatabaseService();
            FormattingService _formattingService = new FormattingService();
            DataLoadingService _dataLoadingService = new DataLoadingService(_databaseService, _formattingService);

            _dataLoadingService.CreateDefaultDatabase();

            MainViewModel mainViewModel = new MainViewModel(_databaseService, _dataLoadingService, _formattingService);

            Window mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
