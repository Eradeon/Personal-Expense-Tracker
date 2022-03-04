using System;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Main
{
    internal class CloseMessageBoxCommand : BaseCommand
    {
        private readonly MessageBoxStore _messageBoxStore;

        public CloseMessageBoxCommand(MessageBoxStore messageBoxStore)
        {
            _messageBoxStore = messageBoxStore;
        }

        public override void Execute(object? parameter)
        {
            _messageBoxStore.HideMessageBox();
        }
    }
}
