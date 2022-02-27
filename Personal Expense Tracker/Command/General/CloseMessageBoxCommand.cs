using System;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.General
{
    internal class CloseMessageBoxCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;

        public CloseMessageBoxCommand(HomeViewModel mainViewModel)
        {
            _homeViewModel = mainViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.MessageBoxService.HideMessageBox();
        }
    }
}
