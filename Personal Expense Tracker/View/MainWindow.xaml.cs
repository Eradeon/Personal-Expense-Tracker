using System;
using System.Windows;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using System.Windows.Interop;

namespace Personal_Expense_Tracker.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowPlacementService _windowPlacementService;
        private readonly ConfigurationService _configurationService;

        public MainWindow(object dataContext, ConfigurationService configurationService)
        {
            this.DataContext = dataContext;

            _windowPlacementService = new WindowPlacementService();
            _configurationService = configurationService;

            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _windowPlacementService.SetPlacement(new WindowInteropHelper(this).Handle, _configurationService.GetStringFromConfig("window_placement"));
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            _configurationService.UpdateAppSettings
            (
                "window_placement",
                _windowPlacementService.GetPlacement(new WindowInteropHelper(this).Handle)
            );
        }
    }
}
