using System;
using Personal_Expense_Tracker.Service;
using Personal_Expense_Tracker.ViewModel;

namespace Personal_Expense_Tracker.Command
{
    internal class DeleteCategoryCommand : BaseCommand
    {
        private readonly MainViewModel _mainViewModel;
        private readonly DatabaseService _databaseService;

        public DeleteCategoryCommand(MainViewModel mainViewModel, DatabaseService databaseService)
        {
            _mainViewModel = mainViewModel;
            _databaseService = databaseService;
        }

        public override void Execute(object? parameter)
        {
            if (_mainViewModel.DeleteCategoryConfirmation)
            {
                if (_mainViewModel.CategoryCollection.Count > 1 && _mainViewModel.SelectedEditCategory != null)
                {
                    _databaseService.DeleteCategory
                    (
                        _mainViewModel.SelectedEditCategory.Id,
                        _mainViewModel.SelectedEditCategory.Name
                    );

                    int collectionId = _mainViewModel.CategoryCollection.IndexOf(_mainViewModel.SelectedEditCategory);

                    if (collectionId > 0)
                    {
                        _mainViewModel.SelectedCategory = _mainViewModel.CategoryCollection[0];
                        _mainViewModel.SelectedEditCategory = _mainViewModel.CategoryCollection[0];
                    }
                    else
                    {
                        _mainViewModel.SelectedCategory = _mainViewModel.CategoryCollection[1];
                        _mainViewModel.SelectedEditCategory = _mainViewModel.CategoryCollection[1];
                    }

                    _mainViewModel.CategoryCollection.RemoveAt(collectionId);

                    //Show message..
                    _mainViewModel.DeleteCategoryConfirmation = false;
                }
                else
                {
                    //error - can't have less than 1
                }
            }
            else
            {
                _mainViewModel.DeleteCategoryConfirmation = true;
            }
        }
    }
}
