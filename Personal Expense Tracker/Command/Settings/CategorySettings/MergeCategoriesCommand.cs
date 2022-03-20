using System;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Settings.CategorySettings
{
    internal class MergeCategoriesCommand : BaseCommand
    {
        private readonly CategorySettingsViewModel _categorySettingsViewModel;
        private readonly DatabaseService _databaseService;
        private readonly MessageBoxStore _messageBoxStore;

        public MergeCategoriesCommand(CategorySettingsViewModel categorySettingsViewModel, DatabaseService databaseService, MessageBoxStore messageBoxStore)
        {
            _categorySettingsViewModel = categorySettingsViewModel;
            _databaseService = databaseService;
            _messageBoxStore = messageBoxStore;
        }

        public override void Execute(object? parameter)
        {
            if (_categorySettingsViewModel.SelectedMergeFromCategory != null && _categorySettingsViewModel.SelectedMergeToCategory != null)
            {
                if (_categorySettingsViewModel.SelectedMergeFromCategory != _categorySettingsViewModel.SelectedMergeToCategory)
                {
                    try
                    {
                        var result = _databaseService.MergeTables(_categorySettingsViewModel.SelectedMergeFromCategory.Name, _categorySettingsViewModel.SelectedMergeToCategory.Name);

                        if (result == DatabaseActionResult.Success)
                        {
                            _messageBoxStore.ShowMessageBox(MessageType.Information, string.Concat
                            (
                                "Data byla úspěšně přesunuta z kategorie ",
                                _categorySettingsViewModel.SelectedMergeFromCategory.DisplayName,
                                " do kategorie ",
                                _categorySettingsViewModel.SelectedMergeToCategory.DisplayName,
                                "."
                            ));
                        }
                    }
                    catch
                    {
                        _messageBoxStore.ShowMessageBox(MessageType.Error, "Chyba: Nebylo možné přesunout data.");
                    }
                }
                else
                {
                    _messageBoxStore.ShowMessageBox(MessageType.Warning, "Jsou vybrány 2 stejné kategorie. Musí být vybrány 2 rozdílné kategorie.");
                }
            }
        }
    }
}
