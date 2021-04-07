using System;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.Services;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels.Categories
{
    public class CategoryDetailViewModel: BaseViewModel
    {
        private readonly ICategoryStore _categoryStore;
        private readonly IPageService _pageService;
        public Category Category { get; private set; }
        public Command SaveCommand { get; private set; }

        public CategoryDetailViewModel(CategoryViewModel viewModel, ICategoryStore categoryStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _categoryStore = categoryStore;
            SaveCommand = new Command(async () => await Save());
            Category = new Category
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,                
            };
        }

        async Task Save()
        {
            if (String.IsNullOrWhiteSpace(Category.Name))
            {
                await _pageService.DisplayAlert("Error", "Please enter the name.", "OK");
                return;
            }
            if (Category.Id == 0)
            {
                await _categoryStore.AddCategory(Category);
                MessagingCenter.Send(this, Events.CategoryAdded, Category);
            }
            else
            {
                await _categoryStore.UpdateCategory(Category);
                MessagingCenter.Send(this, Events.CategoryUpdated, Category);
            }
            await _pageService.PopAsync();
        }
    }
}
