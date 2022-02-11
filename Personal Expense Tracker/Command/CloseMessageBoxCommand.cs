using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class CloseMessageBoxCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

        public CloseMessageBoxCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.MessageBoxService.HideMessageBox();
        }
    }
}
