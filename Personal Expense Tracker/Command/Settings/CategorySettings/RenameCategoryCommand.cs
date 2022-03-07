using System;
using System.ComponentModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Settings.CategorySettings
{
    internal class RenameCategoryCommand : BaseCommand
    {
        private readonly CategorySettingsViewModel _categorySettingsViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        public RenameCategoryCommand(CategorySettingsViewModel categorySettingsViewModel, DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _categorySettingsViewModel = categorySettingsViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;
            _messageBoxStore = messageBoxStore;

            _categorySettingsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_categorySettingsViewModel.RenamedCategoryName) &&
                _formattingService.FormatCategoryTableName(_categorySettingsViewModel.RenamedCategoryName) != "_expenses" &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            string newTableName = _formattingService.FormatCategoryTableName(_categorySettingsViewModel.RenamedCategoryName);

            if (!_categorySettingsViewModel.CategoryExists(newTableName) && _categorySettingsViewModel.SelectedRenameCategory != null)
            {
                string oldDisplayName = _categorySettingsViewModel.SelectedRenameCategory.DisplayName;
                string newDisplayName = _formattingService.FormatCategoryDisplayName(_categorySettingsViewModel.RenamedCategoryName);

                _databaseService.RenameTable
                (
                    _categorySettingsViewModel.SelectedRenameCategory.Id,
                    _categorySettingsViewModel.SelectedRenameCategory.Name,
                    newTableName,
                    newDisplayName
                );

                _categorySettingsViewModel.SelectedRenameCategory.Name = newTableName;
                _categorySettingsViewModel.SelectedRenameCategory.DisplayName = newDisplayName;

                _categorySettingsViewModel.RenamedCategoryName = string.Empty;
                _categorySettingsViewModel.DeleteCategoryConfirmation = false;

                _messageBoxStore.ShowMessageBox(MessageType.Information, $"Kategorie \"{oldDisplayName}\" byla úspěšně přejmenována na \"{newDisplayName}\".");
            }
            else
            {
                _messageBoxStore.ShowMessageBox(MessageType.Warning, $"Kategorie \"{_categorySettingsViewModel.RenamedCategoryName}\" již existuje.");
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_categorySettingsViewModel.RenamedCategoryName))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
