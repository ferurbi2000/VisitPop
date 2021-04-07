using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.Services;
using VisitPop.Mobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels
{
    public class ContactsPageViewModel: BaseViewModel
    {
        private ContactViewModel _selectedContact;
        private IContactStore _contactStore;
        private IPageService _pageService;

        private bool _isDataLoaded;

        public ObservableCollection<ContactViewModel> Contacts { get;  private set; }
            = new ObservableCollection<ContactViewModel>();

        public ContactViewModel SelectedContact 
        { 
            get => _selectedContact;
            set { SetProperty(ref _selectedContact, value); }
        }

        public Command LoadDataCommand { get; private set; }
        public Command AddContactCommand { get; private set; }
        public Command SelectContactCommand { get; private set; }
        public Command DeleteContactCommand { get; private set; }
        public Command CallContactCommand { get; private set; }

        
        public ContactsPageViewModel(IContactStore contactStore, IPageService pageService)
        {
            
            MessagingCenter.Subscribe<ContactsDetailViewModel, Person>
               (this, Events.ContactAdded, OnContactAdded);

            MessagingCenter.Subscribe<ContactsDetailViewModel, Person>
                (this, Events.ContactUpdated, OnContactUpdated);


            _contactStore = contactStore;
            _pageService = pageService;

            LoadDataCommand = new Command(async () => await LoadData());
            AddContactCommand = new Command(async () => await AddContact());
            SelectContactCommand = new Command<ContactViewModel>(async c => await SelectContact(c));
            DeleteContactCommand = new Command<ContactViewModel>(async c => await DeleteContact(c));
            CallContactCommand = new Command<ContactViewModel>(async c => await CallContact(c));
        }

       

        private void OnContactAdded(ContactsDetailViewModel source, Person contact)
        {
            Contacts.Add(new ContactViewModel(contact));
        }

        private void OnContactUpdated(ContactsDetailViewModel source, Person contact)
        {
            var contactInList = Contacts.Single(c => c.Id == contact.Id);

            contactInList.Id = contact.Id;
            contactInList.Nombres = contact.Nombres;
            contactInList.Apellidos = contact.Apellidos;
            contactInList.DocIdentidad = contact.DocIdentidad;
            contactInList.Telefono1 = contact.Telefono1;
            
        }

        private async Task AddContact()
        {
            await _pageService.PushAsync(new ContactsDetailPage(new ContactViewModel()));
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;
            //Contacts.Clear();
            var contacts = await _contactStore.GetContactsAsync();
            foreach (var contact in contacts)
            {
                Contacts.Add(new ContactViewModel(contact));
            }
        }

        private async Task SelectContact(ContactViewModel contact)
        {
            if (contact == null)
                return;

            SelectedContact = null;
            await _pageService.PushAsync(new ContactsDetailPage(contact));
        }

        private async Task DeleteContact(ContactViewModel contactViewModel)
        {
            if(await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {contactViewModel.FullName}?", "Yes", "No"))
            {
                Contacts.Remove(contactViewModel);

                var contact = await _contactStore.GetContact(contactViewModel.Id);
                await _contactStore.DeleteContact(contact);
            }
        }

        private async Task CallContact(ContactViewModel contactViewModel)
        {
            if(contactViewModel != null)
            {
                var message = contactViewModel.FullName + "\n" + contactViewModel.Telefono1;
                bool isCall = await _pageService.DisplayAlert("Do you really want to Call?", message, "Call", "Cancel");
                if (isCall)
                {
                    //Go to Contacts App
                    //Device.OpenUri(new Uri("tel:" + contactViewModel.Title));
                    await Launcher.CanOpenAsync(new Uri("tel:" + contactViewModel.Telefono1));
                }
            }
        }
    }
}
