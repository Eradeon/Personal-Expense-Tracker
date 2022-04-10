using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Settings.CategorySettings
{
    public class DeleteCategoryCommand : BaseCommand
    {
        private readonly CategorySettingsViewModel _categorySettingsViewModel;

        public DeleteCategoryCommand(CategorySettingsViewModel categorySettingsViewModel)
        {
            _categorySettingsViewModel = categorySettingsViewModel;
        }

        public override void Execute(object? parameter)
        {
            _categorySettingsViewModel.DeleteCategoryConfirmation = true;
        }
    }
}
