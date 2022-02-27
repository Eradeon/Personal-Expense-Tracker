using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command.Home
{
    internal class DeleteCategoryCommand : BaseCommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly DatabaseService _databaseService;

        public DeleteCategoryCommand(HomeViewModel homeViewModel, DatabaseService databaseService)
        {
            _homeViewModel = homeViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_homeViewModel.DeleteCategoryConfirmation)
            {
                if (_homeViewModel.CategoryCollection.Count > 1 && _homeViewModel.SelectedEditCategory != null)
                {
                    string categoryName = _homeViewModel.SelectedEditCategory.DisplayName;

                    _databaseService.DeleteCategory
                    (
                        _homeViewModel.SelectedEditCategory.Id,
                        _homeViewModel.SelectedEditCategory.Name
                    );

                    int collectionId = _homeViewModel.CategoryCollection.IndexOf(_homeViewModel.SelectedEditCategory);

                    if (collectionId > 0)
                    {
                        _homeViewModel.SelectedCategory = _homeViewModel.CategoryCollection[0];
                        _homeViewModel.SelectedEditCategory = _homeViewModel.CategoryCollection[0];
                    }
                    else
                    {
                        _homeViewModel.SelectedCategory = _homeViewModel.CategoryCollection[1];
                        _homeViewModel.SelectedEditCategory = _homeViewModel.CategoryCollection[1];
                    }

                    _homeViewModel.CategoryCollection.RemoveAt(collectionId);

                    _homeViewModel.MessageBoxService.ShowMessageBox(MessageType.Information, $"Kategorie {categoryName} byla úspěšně odstraněna.");
                    _homeViewModel.DeleteCategoryConfirmation = false;
                }
                else
                {
                    _homeViewModel.MessageBoxService.ShowMessageBox(MessageType.Warning, "Nelze odstranit výchozí kategoii. Vytvořte novou kategorii a poté odstraňte tuto.");
                }
            }
            else
            {
                _homeViewModel.DeleteCategoryConfirmation = true;
            }
        }
    }
}
