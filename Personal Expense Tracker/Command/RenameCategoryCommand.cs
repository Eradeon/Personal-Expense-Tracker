using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class RenameCategoryCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public RenameCategoryCommand(MainViewModel mainViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_mainViewModel.RenamedCategoryName) &&
                _formattingService.FormatCategoryTableName(_mainViewModel.RenamedCategoryName) != "_expenses" &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (!_mainViewModel.CategoryExists(_mainViewModel.RenamedCategoryTableName))
            {
                string newDisplayName = _formattingService.FormatCategoryDisplayName(_mainViewModel.RenamedCategoryName);

                _databaseService.RenameTable
                (
                    _mainViewModel.GetSelectedCategoryTableId(true),
                    _mainViewModel.GetSelectedCategoryTableName(true),
                    _mainViewModel.RenamedCategoryTableName,
                    newDisplayName
                    
                );

                _mainViewModel.RenameCategoryInCollection(newDisplayName);

                _mainViewModel.RenamedCategoryName = string.Empty;
                _mainViewModel.DeleteCategoryConfirmation = false;

                //message - renamed
            }
            else
            {
                //message - category exists
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainViewModel.RenamedCategoryName))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
