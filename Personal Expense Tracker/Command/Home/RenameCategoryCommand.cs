using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class RenameCategoryCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;

        public RenameCategoryCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;

            _homeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_homeViewModel.RenamedCategoryName) &&
                _formattingService.FormatCategoryTableName(_homeViewModel.RenamedCategoryName) != "_expenses" &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            string newTableName = _formattingService.FormatCategoryTableName(_homeViewModel.RenamedCategoryName);

            if (!_homeViewModel.CategoryExists(newTableName) && _homeViewModel.SelectedEditCategory != null)
            {
                string newDisplayName = _formattingService.FormatCategoryDisplayName(_homeViewModel.RenamedCategoryName);

                _databaseService.RenameTable
                (
                    _homeViewModel.SelectedEditCategory.Id,
                    _homeViewModel.SelectedEditCategory.Name,
                    newTableName,
                    newDisplayName
                    
                );

                _homeViewModel.SelectedEditCategory.Name = newTableName;
                _homeViewModel.SelectedEditCategory.DisplayName = newDisplayName;

                _homeViewModel.RenamedCategoryName = string.Empty;
                _homeViewModel.DeleteCategoryConfirmation = false;

                _homeViewModel.MessageBoxService.ShowMessageBox(MessageType.Information, "Kategorie byla úspěšně přejmenována.");
            }
            else
            {
                _homeViewModel.MessageBoxService.ShowMessageBox(MessageType.Warning, "Kategorie s tímto názvem již existuje.");
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_homeViewModel.RenamedCategoryName))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
