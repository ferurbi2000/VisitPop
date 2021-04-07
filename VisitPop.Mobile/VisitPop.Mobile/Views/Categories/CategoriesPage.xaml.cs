using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Services;
using VisitPop.Mobile.ViewModels;
using VisitPop.Mobile.ViewModels.Categories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisitPop.Mobile.Views.Categories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        public CategoriesPage()
        {
            var categoryStore = new CategoryStore();
            var pageService = new PageService();
            ViewModel = new CategoriesPageViewModel(categoryStore, pageService);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        void OnCategorySelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectCategoryCommand.Execute(e.SelectedItem);
        }

        public CategoriesPageViewModel ViewModel
        {
            get { return BindingContext as CategoriesPageViewModel; }
            set { BindingContext = value; }
        }
    }
}