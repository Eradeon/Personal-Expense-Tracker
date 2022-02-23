using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
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
            string newTableName = _formattingService.FormatCategoryTableName(_mainViewModel.RenamedCategoryName);

            if (!_mainViewModel.CategoryExists(newTableName) && _mainViewModel.SelectedEditCategory != null)
            {
                string newDisplayName = _formattingService.FormatCategoryDisplayName(_mainViewModel.RenamedCategoryName);

                _databaseService.RenameTable
                (
                    _mainViewModel.SelectedEditCategory.Id,
                    _mainViewModel.SelectedEditCategory.Name,
                    newTableName,
                    newDisplayName
                    
                );

                _mainViewModel.SelectedEditCategory.Name = newTableName;
                _mainViewModel.SelectedEditCategory.DisplayName = newDisplayName;

                _mainViewModel.RenamedCategoryName = string.Empty;
                _mainViewModel.DeleteCategoryConfirmation = false;

                _mainViewModel.MessageBoxService.ShowMessageBox(MessageType.Information, "Kategorie byla úspěšně přejmenována.");
            }
            else
            {
                _mainViewModel.MessageBoxService.ShowMessageBox(MessageType.Warning, "Kategorie s tímto názvem již existuje.");
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
