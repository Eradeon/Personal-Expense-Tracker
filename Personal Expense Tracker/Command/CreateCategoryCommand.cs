using System;
using System.Windows;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Extension;
using System.ComponentModel;

namespace Personal_Expense_Tracker.Command
{
    internal class CreateCategoryCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public CreateCategoryCommand(MainViewModel mainViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_mainViewModel.NewCategoryName) &&
                _formattingService.FormatCategoryTableName(_mainViewModel.NewCategoryName) != "_expenses" &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            string tableName = _formattingService.FormatCategoryTableName(_mainViewModel.NewCategoryName);

            if (!_mainViewModel.CategoryExists(tableName))
            {
                try
                {
                    _databaseService.CreateExpenseTable(tableName);

                    string displayName = _formattingService.FormatCategoryDisplayName(_mainViewModel.NewCategoryName);

                    int newCategoryId = _databaseService.InsertCategory
                    (
                        tableName,
                        displayName,
                        _mainViewModel.NewCategoryGroupByMonth.ToInt()
                    );

                    _mainViewModel.CategoryCollection.Add(new CategoryViewModel(new Category
                    (
                        newCategoryId,
                        tableName,
                        displayName,
                        _mainViewModel.NewCategoryGroupByMonth
                    )));

                    _mainViewModel.NewCategoryName = string.Empty;
                    _mainViewModel.NewCategoryGroupByMonth = false;
                    _mainViewModel.DeleteCategoryConfirmation = false;

                    //TODO: Message - category created
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                //TODO: Error - category already exists
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainViewModel.NewCategoryName))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
