using System;
using Personal_Expense_Tracker.Stores;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.General
{
    public class NavigateCommand<TViewModel> : BaseCommand
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
