using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Settings.CategorySettings
{
    internal class CancelDeleteCategoryCommand : BaseCommand
    {
        private readonly CategorySettingsViewModel _categorySettingsViewModel;

        public CancelDeleteCategoryCommand(CategorySettingsViewModel categorySettingsViewModel)
        {
            _categorySettingsViewModel = categorySettingsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _categorySettingsViewModel.DeleteCategoryConfirmation = false;
        }
    }
}
