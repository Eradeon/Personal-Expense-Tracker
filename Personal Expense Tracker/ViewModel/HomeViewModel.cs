using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using System.Globalization;
using Personal_Expense_Tracker.Command.General;
using Personal_Expense_Tracker.Command.Home;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class HomeViewModel : BaseViewModel
    {
        #region Services & Stores
        private readonly DatabaseService _databaseService;
        public readonly MessageBoxStore MessageBoxStore;
        private readonly StatisticsService _statisticsService;
        #endregion Services & Stores

        #region View Controls Properties
        private string _datagridAmountHeader = string.Empty;
        public string DatagridAmountHeader
        {
            get { return _datagridAmountHeader; }
            set { _datagridAmountHeader = value; RaisePropertyChanged(); }
        }
        #endregion View Controls Properties

        #region Collection Properties
        private ObservableCollection<CategoryViewModel> _categoryCollection;
        public ObservableCollection<CategoryViewModel> CategoryCollection
        {
            get { return _categoryCollection; }
            set { _categoryCollection = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<int> _yearList = new();
        public ObservableCollection<int> YearList
        {
            get { return _yearList; }
            set { _yearList = value; RaisePropertyChanged(); }
        }

        private Dictionary<string, string> _monthList;
        public Dictionary<string, string> MonthList
        {
            get { return _monthList; }
            set { _monthList = value; RaisePropertyChanged(); }
        }

        private FullyObservableCollection<ExpenseViewModel> _expenseCollection;
        public FullyObservableCollection<ExpenseViewModel> ExpenseCollection
        {
            get { return _expenseCollection; }
            set { _expenseCollection = value; RaisePropertyChanged(); }
        }

        private ICollectionView _expenseCollectionView;
        public ICollectionViewLiveShaping ExpenseLiveCollectionView { get; set; }
        #endregion Collection Properties

        #region Selected Item Properties
        private CategoryViewModel? _selectedCategory;
        public CategoryViewModel? SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged(); }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; RaisePropertyChanged(); }
        }

        private int _selectedMonth = DateTime.Now.Month - 1;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set { _selectedMonth = value; RaisePropertyChanged(); }
        }

        private bool _groupByMonth;
        public bool GroupByMonth
        {
            get { return _groupByMonth; }
            set { _groupByMonth = value; RaisePropertyChanged(); }
        }
        #endregion Selected Item Properties

        #region Expense Input Properties
        private DateTime _expenseDate = DateTime.Now;
        public DateTime ExpenseDate
        {
            get { return _expenseDate; }
            set { _expenseDate = value; RaisePropertyChanged(); }
        }

        private string _expenseName = string.Empty;
        public string ExpenseName
        {
            get { return _expenseName; }
            set { _expenseName = value; RaisePropertyChanged(); }
        }

        private string _expenseAmount = string.Empty;
        public string ExpenseAmount
        {
            get { return _expenseAmount; }
            set
            {
                if (double.TryParse(value, out double temp) || value == string.Empty)
                {
                    _expenseAmount = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion Expense Input Properties

        #region Expense Management Properties
        //General
        private ExpenseViewModel? _selectedRow;
        public ExpenseViewModel? SelectedRow
        {
            get { return _selectedRow; }
            set { _selectedRow = value; RaisePropertyChanged(); }
        }

        //Modal
        private bool _deleteExpenseModalVisible = false;
        public bool DeleteExpenseModalVisible
        {
            get { return _deleteExpenseModalVisible; }
            set { _deleteExpenseModalVisible = value; RaisePropertyChanged(); }
        }

        private bool _editExpenseModalVisible = false;
        public bool EditExpenseModalVisible
        {
            get { return _editExpenseModalVisible; }
            set { _editExpenseModalVisible = value; RaisePropertyChanged(); }
        }

        //Delete
        private bool _deleteExpenseConfirmation = false;
        public bool DeleteExpenseConfirmation
        {
            get { return _deleteExpenseConfirmation; }
            set { _deleteExpenseConfirmation = value; RaisePropertyChanged(); }
        }

        //Edit
        private bool _editExpenseConfirmation = false;
        public bool EditExpenseConfirmation
        {
            get { return _editExpenseConfirmation; }
            set { _editExpenseConfirmation = value; RaisePropertyChanged(); }
        }

        private DateTime _editExpenseDate;
        public DateTime EditExpenseDate
        {
            get { return _editExpenseDate; }
            set { _editExpenseDate = value; RaisePropertyChanged(); }
        }

        private string _editExpenseName = string.Empty;
        public string EditExpenseName
        {
            get { return _editExpenseName; }
            set { _editExpenseName = value; RaisePropertyChanged(); }
        }

        private string _editExpenseAmount = string.Empty;
        public string EditExpenseAmount
        {
            get { return _editExpenseAmount; }
            set
            {
                if (double.TryParse(value, out double temp) || value == string.Empty)
                {
                    _editExpenseAmount = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion Expense Management Properties

        #region Statistics Properties
        public bool LoadingExpenses = false;

        private ObservableCollection<StatisticCardViewModel> _statisticsCardsCollection;
        public ObservableCollection<StatisticCardViewModel> StatisticsCardsCollection
        {
            get { return _statisticsCardsCollection; }
            set { _statisticsCardsCollection = value; RaisePropertyChanged(); }
        }

        //Group name || Group sum amount || Item count in group || Group sum % of total sum || Number of (100 - % number)
        private List<Tuple<string, string, int, GridLength, GridLength>>? _graphData;
        public List<Tuple<string, string, int, GridLength, GridLength>>? GraphData
        {
            get { return _graphData; }
            set { _graphData = value; RaisePropertyChanged(); }
        }

        private Tuple<int, int, int, int, int>? _xAxisLabels;
        public Tuple<int, int, int, int, int>? XAxisLabels
        {
            get { return _xAxisLabels; }
            set { _xAxisLabels = value; RaisePropertyChanged(); }
        }
        #endregion Statistics Properties

        #region Commands
        public ICommand CategoryChanged { get; }
        public ICommand UpdateCategoryGroupByMonth { get; }

        public ICommand LoadExpenses { get; }
        public ICommand AddExpense { get; }
        public ICommand DeleteExpense { get; }
        public ICommand EditExpense { get; }
        public ICommand HideDeleteExpense { get; }
        public ICommand HideEditExpense { get; }

        public ICommand DefaultDataGridSorting { get; }

        public ICommand UnfocusElementUponMouseClick { get; }
        #endregion Commands

        public HomeViewModel(DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _databaseService = databaseService;
            MessageBoxStore = messageBoxStore;
            _statisticsService = new StatisticsService(this);

            //Setting up the basics
            _datagridAmountHeader = string.Concat("Částka v ", CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);

            _categoryCollection = LoadCategories();
            _selectedCategory = _categoryCollection[0];

            _monthList = LoadMonths();
            _statisticsCardsCollection = LoadStatisticsCards();

            _expenseCollection = new FullyObservableCollection<ExpenseViewModel>();
            _expenseCollectionView = CollectionViewSource.GetDefaultView(ExpenseCollection);
            ExpenseLiveCollectionView = (ICollectionViewLiveShaping)_expenseCollectionView;
            ExpenseLiveCollectionView.IsLiveSorting = true;

            //Commands
            CategoryChanged = new CategoryChangedCommand(this);
            UpdateCategoryGroupByMonth = new UpdateCategoryGroupByMonthCommand(this, databaseService);

            LoadExpenses = new LoadExpenseDataCommand(this, databaseService);
            AddExpense = new CreateExpenseCommand(this, databaseService, formattingService);
            DeleteExpense = new DeleteExpenseCommand(this, databaseService, messageBoxStore);
            EditExpense = new EditExpenseCommand(this, databaseService, formattingService, messageBoxStore);
            HideDeleteExpense = new HideDeleteExpenseModalCommand(this);
            HideEditExpense = new HideEditExpenseModalCommand(this);

            DefaultDataGridSorting = new DefaultDataGridSortingCommand();

            UnfocusElementUponMouseClick = new UnfocusElementUponMouseClickCommand();

            //Loading expense data
            CategoryChanged.Execute(null);
            LoadExpenses.Execute(null);
            _statisticsService.CalculateHomeStatistics();

            //Setting up event listeners
            ExpenseCollection.CollectionChanged += ExpenseCollectionChangedEventHandler;
            ExpenseCollection.ItemPropertyChanged += ExpenseCollectionItemChangedEventHandler;
        }

        private void ExpenseCollectionChangedEventHandler(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!LoadingExpenses)
                _statisticsService.CalculateHomeStatistics();
        }

        private void ExpenseCollectionItemChangedEventHandler(object? sender, ItemPropertyChangedEventArgs e)
        {
            if (!LoadingExpenses)
                _statisticsService.CalculateHomeStatistics();
        }

        public bool CategoryExists(string tableName)
        {
            foreach (var category in _categoryCollection)
            {
                if (category.Name == tableName)
                    return true;
            }

            return false;
        }

        private ObservableCollection<CategoryViewModel> LoadCategories()
        {
            DataTable dataTable = _databaseService.QueryDatabase("SELECT * FROM categories;");
            ObservableCollection<CategoryViewModel> categoryCollection = new();

            foreach (DataRow row in dataTable.Rows)
            {
                categoryCollection.Add(new CategoryViewModel(new Category
                (
                    int.Parse(row["category_id"].ToString()!),
                    row["category_name"].ToString()!,
                    row["category_display_name"].ToString()!,
                    int.Parse(row["group_by_month"].ToString()!).ToBool()
                )));
            }

            return categoryCollection;
        }

        public void LoadYears()
        {
            if (_yearList.Count > 0)
                YearList.Clear();

            if (_selectedCategory != null)
            {
                int currentYear = DateTime.Now.Year;

                DataTable dataTable = _databaseService.QueryDatabase($"SELECT DISTINCT strftime('%Y', expense_date) year_list FROM {_selectedCategory.Name} ORDER BY year_list ASC;");

                if (dataTable == null || dataTable.Rows.Count == 0)
                    YearList.Add(currentYear);
                else
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        YearList.Add(int.Parse(row["year_list"].ToString()!));
                    }

                    AddYear(currentYear);
                }

                SelectedYear = currentYear;
            }
        }

        public void AddYear(int year)
        {
            int j = 1;

            for (int i = 0; i < _yearList.Count; i++)
            {
                if (_yearList[i] == year)
                    break;
                else if (_yearList[0] > year)
                {
                    YearList.Insert(0, year);
                    break;
                }
                else if (_yearList[_yearList.Count-1] < year)
                {
                    YearList.Add(year);
                    break;
                }
                else if (_yearList[i] < year && _yearList[j] > year)
                {
                    YearList.Insert(j, year);
                    break;
                }

                j++;
            }
        }

        private Dictionary<string, string> LoadMonths()
        {
            Dictionary<string, string> months = new();

            for (int i = 1; i <= 12; i++)
            {
                if (i <= 9)
                {
                    months.Add("0" + i, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)));
                }
                else
                {
                    months.Add(i.ToString(), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)));
                }
            }

            return months;
        }

        private ObservableCollection<StatisticCardViewModel> LoadStatisticsCards()
        {
            ObservableCollection<StatisticCardViewModel> result = new();

            result.Add(new StatisticCardViewModel(new StatisticCard("\ue885", "Nejčastější položka", "N/A", "N/A", "Kolikrát se položka vyskytuje ve vybraném odbobí.")));
            result.Add(new StatisticCardViewModel(new StatisticCard("\ue24a", "Celkové výdaje", "N/A", "N/A", "Celkový počet položek ve vybraném období.")));
            result.Add(new StatisticCardViewModel(new StatisticCard("\ue8e3", "Nejnižší výdaj", "N/A", "N/A", "Název položky s nejnižším výdajem.")));
            result.Add(new StatisticCardViewModel(new StatisticCard("\ue8e5", "Nejvyšší výdaj", "N/A", "N/A", "Název položky s nejvyšším výdajem.")));
            result.Add(new StatisticCardViewModel(new StatisticCard("\ue4fc", "Průměrný výdaj", "N/A", "N/A", "Průměrný výdaj na 1 den.")));

            return result;
        }
    }
}