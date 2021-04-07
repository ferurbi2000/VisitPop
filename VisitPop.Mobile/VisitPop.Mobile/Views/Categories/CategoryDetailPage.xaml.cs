using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Services;
using VisitPop.Mobile.ViewModels.Categories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisitPop.Mobile.Views.Categories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryDetailPage : ContentPage
    {
        public CategoryDetailPage(CategoryViewModel viewModel)
        {
            InitializeComponent();
            var categoryStore = new CategoryStore();
            var pageService = new PageService();

            Title = (viewModel.Description == null) ? "New Category" : "Edit Category";
            BindingContext = new CategoryDetailViewModel(viewModel ?? new CategoryViewModel(), categoryStore, pageService);
        }
    }
}