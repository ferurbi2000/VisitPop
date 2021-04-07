using VisitPop.Mobile.Services;
using VisitPop.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisitPop.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsDetailPage : ContentPage
    {
        public ContactsDetailPage(ContactViewModel viewModel)
        {
            InitializeComponent();
            var contactStore = new ContactStore();
            var pageService = new PageService();

            Title = (viewModel.Telefono1 == null) ? "New Contact" : "Edit Contact";
            BindingContext = new ContactsDetailViewModel(viewModel ?? new ContactViewModel(), contactStore, pageService);
        }

        
    }
}