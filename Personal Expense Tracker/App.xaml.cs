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

            FormattingService _formattingService = new FormattingService();
            DatabaseService _databaseService = new DatabaseService(_formattingService);

            _databaseService.CreateDefaultDatabase();

            MainViewModel mainViewModel = new MainViewModel(_databaseService, _formattingService);

            Window mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
