using System;
using System.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Command.General;
using Personal_Expense_Tracker.Command.Settings.CategorySettings;

namespace Personal_Expense_Tracker.ViewModel
{
    internal class CategorySettingsViewModel : BaseViewModel
    {
        #region Services & Stores
        private readonly DatabaseService _databaseService;
        #endregion Services & Stores

        #region Collections
        private ObservableCollection<CategoryViewModel> _categoryCollection;
        public ObservableCollection<CategoryViewModel> CategoryCollection
        {
            get { return _categoryCollection; }
            set { _categoryCollection = value; RaisePropertyChanged(); }
        }
        #endregion Collections

        #region Create Category Properties
        private string _newCategoryName = string.Empty;
        public string NewCategoryName
        {
            get { return _newCategoryName; }
            set { _newCategoryName=value; RaisePropertyChanged(); }
        }

        private bool _newCategoryGroupByMonth = false;
        public bool NewCategoryGroupByMonth
        {
            get { return _newCategoryGroupByMonth; }
            set { _newCategoryGroupByMonth = value; RaisePropertyChanged(); }
        }
        #endregion Create Category Properties

        #region Rename Category Properties
        private CategoryViewModel? _selectedRenameCategory;
        public CategoryViewModel? SelectedRenameCategory
        {
            get { return _selectedRenameCategory; }
            set { _selectedRenameCategory = value; RaisePropertyChanged(); }
        }

        private string _renamedCategoryName = string.Empty;
        public string RenamedCategoryName
        {
            get { return _renamedCategoryName; }
            set { _renamedCategoryName = value; RaisePropertyChanged(); }
        }
        #endregion Rename Category Properties

        #region Delete Category Properties
        private CategoryViewModel? _selectedDeleteCategory;
        public CategoryViewModel? SelectedDeleteCategory
        {
            get { return _selectedDeleteCategory; }
            set { _selectedDeleteCategory = value; RaisePropertyChanged(); }
        }

        private bool _deleteCategoryConfirmation = false;
        public bool DeleteCategoryConfirmation
        {
            get { return _deleteCategoryConfirmation; }
            set { _deleteCategoryConfirmation = value; RaisePropertyChanged(); }
        }
        #endregion Delete Category Properties

        #region Merge Category Properties
        private CategoryViewModel? _selectedMergeFromCategory;
        public CategoryViewModel? SelectedMergeFromCategory
        {
            get { return _selectedMergeFromCategory; }
            set { _selectedMergeFromCategory = value; RaisePropertyChanged(); }
        }

        private CategoryViewModel? _selectedMergeToCategory;
        public CategoryViewModel? SelectedMergeToCategory
        {
            get { return _selectedMergeToCategory; }
            set { _selectedMergeToCategory = value; RaisePropertyChanged(); }
        }
        #endregion Merge Category Properties

        #region Commands
        public ICommand CreateCategoryCommand { get; }
        public ICommand RenameCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand CancelDeleteCategoryCommand { get; }
        public ICommand ConfirmDeleteCategoryCommand { get; }
        public ICommand MergeCategoriesCommand { get; }

        public ICommand UnfocusElementUponMouseClickCommand { get; }
        #endregion Commands

        public CategorySettingsViewModel(MessageBoxStore messageBoxStore, FormattingService formattingService, DatabaseService databaseService)
        {
            _databaseService = databaseService;

            _categoryCollection = LoadCategories();

            _selectedRenameCategory = _categoryCollection[0];
            _selectedDeleteCategory = _categoryCollection[0];
            _selectedMergeFromCategory = _categoryCollection[0];
            _selectedMergeToCategory = _categoryCollection[0];

            //Commands
            CreateCategoryCommand = new CreateCategoryCommand(this, databaseService, formattingService, messageBoxStore);
            RenameCategoryCommand = new RenameCategoryCommand(this, databaseService, formattingService, messageBoxStore);
            DeleteCategoryCommand = new DeleteCategoryCommand(this);
            CancelDeleteCategoryCommand = new CancelDeleteCategoryCommand(this);
            ConfirmDeleteCategoryCommand = new ConfirmDeleteCategoryCommand(this, databaseService, messageBoxStore);
            MergeCategoriesCommand = null; //TODO

            UnfocusElementUponMouseClickCommand = new UnfocusElementUponMouseClickCommand();
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
            ObservableCollection<CategoryViewModel> categoryCollection = new ObservableCollection<CategoryViewModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                categoryCollection.Add(new CategoryViewModel(new Category
                (
                    int.Parse(row["category_id"].ToString()),
                    row["category_name"].ToString(),
                    row["category_display_name"].ToString(),
                    int.Parse(row["group_by_month"].ToString()).ToBool()
                )));
            }

            return categoryCollection;
        }
    }
}
