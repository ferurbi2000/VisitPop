using System;
using System.Collections.Generic;
using VisitPop.Mobile.ViewModels;
using VisitPop.Mobile.Views;
using Xamarin.Forms;

namespace VisitPop.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(PersonDetailPage), typeof(PersonDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NewPersonPage), typeof(NewPersonPage));                        
            
        }

    }
}
