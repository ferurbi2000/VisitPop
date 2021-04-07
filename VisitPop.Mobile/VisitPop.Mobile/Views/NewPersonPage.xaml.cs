using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisitPop.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPersonPage : ContentPage
    {
        public Person Item { get; set; }
        public NewPersonPage()
        {
            InitializeComponent();
            BindingContext = new NewPersonViewModel();
        }
    }
}