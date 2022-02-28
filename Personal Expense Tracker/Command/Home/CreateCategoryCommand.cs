using System;
using System.Windows;
using System.ComponentModel;
using Personal_Expense_Tracker.Model;
using Personal_Expense_Tracker.ViewModel;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.Extension;
using Personal_Expense_Tracker.Stores;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class CreateCategoryCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;
        private readonly FormattingService _formattingService;
        private readonly MessageBoxStore _messageBoxStore;

        public CreateCategoryCommand(HomeViewModel homeViewModel, DatabaseService databaseService, FormattingService formattingService, MessageBoxStore messageBoxStore)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
            _formattingService = formattingService;
            _messageBoxStore = messageBoxStore;

            _homeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_homeViewModel.NewCategoryName) &&
                _formattingService.FormatCategoryTableName(_homeViewModel.NewCategoryName) != "_expenses" &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            string tableName = _formattingService.FormatCategoryTableName(_homeViewModel.NewCategoryName);

            if (!_homeViewModel.CategoryExists(tableName))
            {
                try
                {
                    _databaseService.CreateExpenseTable(tableName);

                    string displayName = _formattingService.FormatCategoryDisplayName(_homeViewModel.NewCategoryName);

                    int newCategoryId = _databaseService.InsertCategory
                    (
                        tableName,
                        displayName,
                        _homeViewModel.NewCategoryGroupByMonth.ToInt()
                    );

                    _homeViewModel.CategoryCollection.Add(new CategoryViewModel(new Category
                    (
                        newCategoryId,
                        tableName,
                        displayName,
                        _homeViewModel.NewCategoryGroupByMonth
                    )));

                    _homeViewModel.NewCategoryName = string.Empty;
                    _homeViewModel.NewCategoryGroupByMonth = false;
                    _homeViewModel.DeleteCategoryConfirmation = false;

                    _messageBoxStore.ShowMessageBox(MessageType.Information, $"Kategorie {displayName} byla úspěšně vytvořena.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                _messageBoxStore.ShowMessageBox(MessageType.Warning, "Kategorie s tímto názvem již existuje.");
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_homeViewModel.NewCategoryName))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
