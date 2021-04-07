using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.Services;
using VisitPop.Mobile.Views.Categories;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels.Categories
{
    public class CategoriesPageViewModel: BaseViewModel
    {
        private CategoryViewModel _selectedCategory;
        private ICategoryStore _categoryStore;
        private IPageService _pageService;

        private bool _isDataLoaded;

        public ObservableCollection<CategoryViewModel> Categories { get; private set; }
            = new ObservableCollection<CategoryViewModel>();

        public CategoryViewModel SelectedCategory
        {
            get => _selectedCategory;
            set { SetProperty(ref _selectedCategory, value); }
        }

        public Command LoadDataCommand { get; private set; }
        public Command AddCategoryCommand { get; private set; }
        public Command SelectCategoryCommand { get; private set; }
        public Command DeleteCategoryCommand { get; private set; }
        public Command CallCategoryCommand { get; private set; }

        public CategoriesPageViewModel(ICategoryStore categoryStore, IPageService pageService)
        {

            MessagingCenter.Subscribe<CategoryDetailViewModel, Category>
               (this, Events.CategoryAdded, OnCategoryAdded);

            MessagingCenter.Subscribe<CategoryDetailViewModel, Category>
                (this, Events.CategoryUpdated, OnCategoryUpdated);


            _categoryStore = categoryStore;
            _pageService = pageService;

            LoadDataCommand = new Command(async () => await LoadData());
            AddCategoryCommand = new Command(async () => await AddCategory());
            SelectCategoryCommand = new Command<CategoryViewModel>(async c => await SelectCategory(c));
            DeleteCategoryCommand = new Command<CategoryViewModel>(async c => await DeleteCategory(c));
            CallCategoryCommand = new Command<CategoryViewModel>(async c => await CallCategory(c));
        }



        private void OnCategoryAdded(CategoryDetailViewModel source, Category category)
        {
            Categories.Add(new CategoryViewModel(category));
        }

        private void OnCategoryUpdated(CategoryDetailViewModel source, Category category)
        {
            var categoryInList = Categories.Single(c => c.Id == category.Id);

            categoryInList.Id = category.Id;
            categoryInList.Name = category.Name;
            categoryInList.Description = category.Description;            

        }

        private async Task AddCategory()
        {
            await _pageService.PushAsync(new CategoryDetailPage(new CategoryViewModel()));
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;
            //Contacts.Clear();
            var categories = await _categoryStore.GetCategoriesAsync();
            foreach (var category in categories)
            {
                Categories.Add(new CategoryViewModel(category));
            }
        }

        private async Task SelectCategory(CategoryViewModel category)
        {
            if (category == null)
                return;

            SelectedCategory = null;
            await _pageService.PushAsync(new CategoryDetailPage(category));
        }

        private async Task DeleteCategory(CategoryViewModel categoryViewModel)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {categoryViewModel.FullName}?", "Yes", "No"))
            {
                Categories.Remove(categoryViewModel);

                var category = await _categoryStore.GetCategory(categoryViewModel.Id);
                await _categoryStore.DeleteCategory(category);
            }
        }

        private async Task CallCategory(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel != null)
            {
                var message = categoryViewModel.FullName + "\n" + categoryViewModel.Description;
                bool isCall = await _pageService.DisplayAlert("Do you really want to Call?", message, "Call", "Cancel");
                if (isCall)
                {
                    //Go to Contacts App
                    //Device.OpenUri(new Uri("tel:" + contactViewModel.Title));
                    await Launcher.CanOpenAsync(new Uri("tel:" + categoryViewModel.Description));
                }
            }
        }
    }
}
