using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using System.Globalization;
using Personal_Expense_Tracker.Command.General;
using Personal_Expense_Tracker.Command.Home;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Extension;

namespace Personal_Expense_Tracker.ViewModel
{
    internal enum MessageType
    {
        Information,
        Warning,
        Error
    }

    internal class MainViewModel : BaseViewModel
    {
        #region Services
        private readonly DatabaseService _databaseService;
        private readonly DataLoadingService _dataLoadingService;
        private readonly FormattingService _formattingService;
        public readonly MessageBoxService MessageBoxService;
        private readonly StatisticsService _statisticsService;
        #endregion Services

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

        private List<int> _yearList;
        public List<int> YearList
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

        private CategoryViewModel? _selectedEditCategory;
        public CategoryViewModel? SelectedEditCategory
        {
            get { return _selectedEditCategory; }
            set { _selectedEditCategory = value; RaisePropertyChanged(); }
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

        #region Category Management Properties
        //Modal
        private bool _categoryManagementModalVisible = false;
        public bool CategoryManagementModalVisible
        {
            get { return _categoryManagementModalVisible; }
            set { _categoryManagementModalVisible = value; RaisePropertyChanged(); }
        }

        //Create
        private string _newCategoryName = string.Empty;
        public string NewCategoryName
        {
            get { return _newCategoryName; }
            set { _newCategoryName = value; RaisePropertyChanged(); NewCategoryTableName = _formattingService.FormatCategoryTableName(value); }
        }

        private string _newCategoryTableName = string.Empty;
        public string NewCategoryTableName
        {
            get { return _newCategoryTableName; }
            set
            {
                if (value == "_expenses") { _newCategoryTableName = string.Empty; RaisePropertyChanged(); }
                else { _newCategoryTableName = value; RaisePropertyChanged(); }
            }
        }

        private bool _newCategoryGroupByMonth;
        public bool NewCategoryGroupByMonth
        {
            get { return _newCategoryGroupByMonth; }
            set { _newCategoryGroupByMonth = value; RaisePropertyChanged(); }
        }

        //Delete
        private bool _deleteCategoryConfirmation = false;
        public bool DeleteCategoryConfirmation
        {
            get { return _deleteCategoryConfirmation; }
            set { _deleteCategoryConfirmation = value; RaisePropertyChanged(); }
        }

        //Rename
        private string _renamedCategoryName = string.Empty;
        public string RenamedCategoryName
        {
            get { return _renamedCategoryName; }
            set { _renamedCategoryName = value; RaisePropertyChanged(); RenamedCategoryTableName = _formattingService.FormatCategoryTableName(value); }
        }

        private string _renamedCategoryTableName = string.Empty;
        public string RenamedCategoryTableName
        {
            get { return _renamedCategoryTableName; }
            set
            {
                if (value == "_expenses") { _renamedCategoryTableName = string.Empty; RaisePropertyChanged(); }
                else { _renamedCategoryTableName = value; RaisePropertyChanged(); }
            }
        }
        #endregion Category Management Properties

        #region Message Box Properties
        private bool _messageBoxVisible;
        public bool MessageBoxVisible
        {
            get { return _messageBoxVisible; }
            set { _messageBoxVisible = value; RaisePropertyChanged(); }
        }

        private MessageType _messageBoxType;
        public MessageType MessageBoxType
        {
            get { return _messageBoxType; }
            set { _messageBoxType = value; RaisePropertyChanged(); }
        }

        private string _messageText = string.Empty;
        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; RaisePropertyChanged(); }
        }

        private bool _isMessageBoxClosing = false;
        public bool IsMessageBoxClosing
        {
            get { return _isMessageBoxClosing; }
            set { _isMessageBoxClosing= value; RaisePropertyChanged(); }
        }
        #endregion Message Box Properties

        #region Statistics Properties
        public bool LoadingExpenses = false;

        private List<Tuple<string, int>>? _mostFrequentedExpenses;
        public List<Tuple<string, int>>? MostFrequentedExpenses
        {
            get { return _mostFrequentedExpenses; }
            set { _mostFrequentedExpenses = value; RaisePropertyChanged(); }
        }

        private Tuple<string, int>? _sumExpense;
        public Tuple<string, int>? SumExpense
        {
            get { return _sumExpense; }
            set { _sumExpense = value; RaisePropertyChanged(); }
        }

        private Tuple<string, string>? _meanExpense;
        public Tuple<string, string>? MeanExpense
        {
            get { return _meanExpense; }
            set { _meanExpense = value; RaisePropertyChanged(); }
        }

        private Tuple<string, string>? _minExpense;
        public Tuple<string, string>? MinExpense
        {
            get { return _minExpense; }
            set { _minExpense = value; RaisePropertyChanged(); }
        }

        private Tuple<string, string>? _maxExpense;
        public Tuple<string, string>? MaxExpense
        {
            get { return _maxExpense; }
            set { _maxExpense = value; RaisePropertyChanged(); }
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
        public ICommand ShowCategoryManagement { get; }
        public ICommand HideCategoryManagement { get; }
        public ICommand AddCategory { get; }
        public ICommand DeleteCategory { get; }
        public ICommand CancelDeleteCategory { get; }
        public ICommand RenameCategory { get; }
        public ICommand CategoryChanged { get; }
        public ICommand UpdateCategoryGroupByMonth { get; }

        public ICommand LoadExpenses { get; }
        public ICommand AddExpense { get; }
        public ICommand DeleteExpense { get; }
        public ICommand EditExpense { get; }
        public ICommand HideDeleteExpense { get; }
        public ICommand HideEditExpense { get; }

        public ICommand DefaultDataGridSorting { get; }

        public ICommand CloseMessageBox { get; }

        public ICommand LoseFocusWhenEmptySpaceClicked { get; }
        public ICommand UnfocusElementUponMouseClick { get; }
        #endregion Commands

        public MainViewModel(DatabaseService databaseService, DataLoadingService dataLoadingService, FormattingService formattingService)
        {
            _databaseService = databaseService;
            _dataLoadingService = dataLoadingService;
            _formattingService = formattingService;
            MessageBoxService = new MessageBoxService(this);
            _statisticsService = new StatisticsService(this);

            //Setting up the basics
            _datagridAmountHeader = string.Concat("Částka v ", CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);

            _categoryCollection = _dataLoadingService.LoadCategories();
            _selectedCategory = _categoryCollection[0];
            _selectedEditCategory = _categoryCollection[0];

            _yearList = Enumerable.Range(2014, DateTime.Now.Year - 2014 + 1).Select(y => y).ToList();
            _selectedYear = _yearList.Last();

            _monthList = _dataLoadingService.LoadMonths();

            _expenseCollection = new FullyObservableCollection<ExpenseViewModel>();
            _expenseCollectionView = CollectionViewSource.GetDefaultView(ExpenseCollection);
            ExpenseLiveCollectionView = (ICollectionViewLiveShaping)_expenseCollectionView;
            ExpenseLiveCollectionView.IsLiveSorting = true;

            //Commands
            ShowCategoryManagement = new ShowCategoryModalCommand(this);
            HideCategoryManagement = new HideCategoryModalCommand(this);
            AddCategory = new CreateCategoryCommand(this, databaseService, formattingService);
            DeleteCategory = new DeleteCategoryCommand(this, databaseService);
            CancelDeleteCategory = new CancelDeleteCategoryCommand(this);
            RenameCategory = new RenameCategoryCommand(this, databaseService, formattingService);
            CategoryChanged = new CategoryChangedCommand(this);
            UpdateCategoryGroupByMonth = new UpdateCategoryGroupByMonthCommand(this, databaseService);

            LoadExpenses = new LoadExpenseDataCommand(this, databaseService);
            AddExpense = new CreateExpenseCommand(this, databaseService, formattingService);
            DeleteExpense = new DeleteExpenseCommand(this, databaseService);
            EditExpense = new EditExpenseCommand(this, databaseService, formattingService);
            HideDeleteExpense = new HideDeleteExpenseModalCommand(this);
            HideEditExpense = new HideEditExpenseModalCommand(this);

            DefaultDataGridSorting = new DefaultDataGridSortingCommand();

            CloseMessageBox = new CloseMessageBoxCommand(this);

            LoseFocusWhenEmptySpaceClicked = new LoseFocusWhenEmptySpaceClickedCommand();
            UnfocusElementUponMouseClick = new UnfocusElementUponMouseClickCommand();

            //Loading expense data
            CategoryChanged.Execute(null);
            LoadExpenses.Execute(null);
            _statisticsService.CalculateStatistics();

            //Setting up event listeners
            ExpenseCollection.CollectionChanged += ExpenseCollectionChangedEventHandler;
            ExpenseCollection.ItemPropertyChanged += ExpenseCollectionItemChangedEventHandler;

            //Testing Purposes
            //...
            //Testing Purposes
        }

        private void ExpenseCollectionChangedEventHandler(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!LoadingExpenses)
                _statisticsService.CalculateStatistics();
        }

        private void ExpenseCollectionItemChangedEventHandler(object? sender, ItemPropertyChangedEventArgs e)
        {
            if (!LoadingExpenses)
                _statisticsService.CalculateStatistics();
        }

        public bool CategoryExists(string tableName)
        {
            foreach(var category in _categoryCollection)
            {
                if (category.Name == tableName)
                    return true;
            }

            return false;
        }
    }
}