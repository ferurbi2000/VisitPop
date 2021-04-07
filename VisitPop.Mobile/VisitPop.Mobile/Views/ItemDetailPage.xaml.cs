using System.ComponentModel;
using VisitPop.Mobile.ViewModels;
using Xamarin.Forms;

namespace VisitPop.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}