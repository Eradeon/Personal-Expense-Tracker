using System;
using System.Windows;
using System.ComponentModel;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Settings.CategorySettings
{
    internal class CreateCategoryCommand : BaseCommand
    {
        private readonly CategorySettingsViewModel _categorySettingsViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        public CreateCategoryCommand(CategorySettingsViewModel categorySettingsViewModel, DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _categorySettingsViewModel = categorySettingsViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;
            _messageBoxStore = messageBoxStore;

            _categorySettingsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_categorySettingsViewModel.NewCategoryName) &&
                _formattingService.FormatCategoryTableName(_categorySettingsViewModel.NewCategoryName) != "_expenses" &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            string tableName = _formattingService.FormatCategoryTableName(_categorySettingsViewModel.NewCategoryName);

            if (!_categorySettingsViewModel.CategoryExists(tableName))
            {
                string displayName = _formattingService.FormatCategoryDisplayName(_categorySettingsViewModel.NewCategoryName);

                int newCategoryId = _databaseService.CreateExpenseCategory
                (
                    tableName,
                    displayName,
                    _categorySettingsViewModel.NewCategoryGroupByMonth.ToInt()
                );

                if (newCategoryId > 0)
                {
                    _categorySettingsViewModel.CategoryCollection.Add(new CategoryViewModel(new Category
                    (
                        newCategoryId,
                        tableName,
                        displayName,
                        _categorySettingsViewModel.NewCategoryGroupByMonth
                    )));

                    _messageBoxStore.ShowMessageBox(MessageType.Information, $"Kategorie {displayName} byla úspěšně vytvořena.");
                }

                _categorySettingsViewModel.NewCategoryName = string.Empty;
                _categorySettingsViewModel.NewCategoryGroupByMonth = false;
                _categorySettingsViewModel.DeleteCategoryConfirmation = false;

                if (parameter != null && parameter is UIElement)
                {
                    UIElement element = (UIElement)parameter;

                    if (element.Focusable)
                        element.Focus();
                }
            }
            else
            {
                _messageBoxStore.ShowMessageBox(MessageType.Warning, $"Kategorie \"{_categorySettingsViewModel.NewCategoryName}\" již existuje.");
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_categorySettingsViewModel.NewCategoryName))
            {
                OnCanExecuteChanged();
            }
        }

        public override void Dispose()
        {
            _categorySettingsViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            base.Dispose();
        }
    }
}
