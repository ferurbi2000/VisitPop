using System;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.Services;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels
{
    public class ContactsDetailViewModel: BaseViewModel

    {
        private readonly IContactStore _contactStore;
        private readonly IPageService _pageService;
        public Person Contact { get; private set; }
        public Command SaveCommand { get; private set; }

        public ContactsDetailViewModel(ContactViewModel viewModel, IContactStore contactStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            
            _pageService = pageService;
            _contactStore = contactStore;
            SaveCommand = new Command(async () => await Save());
            Contact = new Person
            {
                Id = viewModel.Id,
                Nombres = viewModel.Nombres,
                Apellidos = viewModel.Apellidos,
                DocIdentidad = viewModel.DocIdentidad,
                Telefono1 = viewModel.Telefono1
            };
        }

        async Task Save()
        {
            if(String.IsNullOrWhiteSpace(Contact.Nombres)&& String.IsNullOrWhiteSpace(Contact.Apellidos))
            {
                await _pageService.DisplayAlert("Error", "Please enter the name.", "OK");
                return;
            }
            if (Contact.Id == 0)
            {
                await _contactStore.AddContact(Contact);
                MessagingCenter.Send(this, Events.ContactAdded, Contact);
            }
            else
            {
                await _contactStore.UpdateContact(Contact);
                MessagingCenter.Send(this, Events.ContactUpdated, Contact);
            }
            await _pageService.PopAsync();
        }
    }
}
