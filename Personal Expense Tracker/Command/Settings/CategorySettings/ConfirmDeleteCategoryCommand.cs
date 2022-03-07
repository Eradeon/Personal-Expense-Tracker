﻿using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Settings.CategorySettings
{
    internal class ConfirmDeleteCategoryCommand : BaseCommand
    {
        private readonly CategorySettingsViewModel _categorySettingsViewModel;
        private readonly DatabaseService _databaseService;
        private readonly MessageBoxStore _messageBoxStore;

        public ConfirmDeleteCategoryCommand(CategorySettingsViewModel categorySettingsViewModel, DatabaseService databaseService, MessageBoxStore messageBoxStore)
        {
            _categorySettingsViewModel = categorySettingsViewModel;
            _databaseService = databaseService;
            _messageBoxStore = messageBoxStore;
        }

        public override void Execute(object? parameter)
        {
            if (_categorySettingsViewModel.DeleteCategoryConfirmation)
            {
                if (_categorySettingsViewModel.CategoryCollection.Count > 1 && _categorySettingsViewModel.SelectedDeleteCategory != null)
                {
                    string categoryName = _categorySettingsViewModel.SelectedDeleteCategory.DisplayName;

                    _databaseService.DeleteCategory
                    (
                        _categorySettingsViewModel.SelectedDeleteCategory.Id,
                        _categorySettingsViewModel.SelectedDeleteCategory.Name
                    );

                    int collectionId = _categorySettingsViewModel.CategoryCollection.IndexOf(_categorySettingsViewModel.SelectedDeleteCategory);

                    _categorySettingsViewModel.CategoryCollection.RemoveAt(collectionId);

                    _categorySettingsViewModel.SelectedDeleteCategory = _categorySettingsViewModel.CategoryCollection[0];
                    _categorySettingsViewModel.SelectedRenameCategory = _categorySettingsViewModel.CategoryCollection[0];
                    _categorySettingsViewModel.SelectedMergeFromCategory = _categorySettingsViewModel.CategoryCollection[0];
                    _categorySettingsViewModel.SelectedMergeToCategory = _categorySettingsViewModel.CategoryCollection[0];

                    _messageBoxStore.ShowMessageBox(MessageType.Information, $"Kategorie {categoryName} byla úspěšně odstraněna.");
                    _categorySettingsViewModel.DeleteCategoryConfirmation = false;
                }
                else
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Warning, "Nelze odstranit výchozí kategoii. Vytvořte, prosím, novou kategorii a poté odstraňte požadovanou kategorii.");
                }
            }
        }
    }
}
