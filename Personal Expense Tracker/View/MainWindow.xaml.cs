using System;
using System.Globalization;
using System.Threading;
using System.Windows;


namespace Personal_Expense_Tracker.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            this.DataContext = dataContext;

            InitializeComponent();

            //Because trying to format DatePicker string in XAML does not work as required
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
    }
}
