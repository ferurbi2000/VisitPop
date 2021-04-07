using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.Views;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        private Person _selectItem;
        public ObservableCollection<Person> Items { get; }
        public Command LoadPersonCommand { get; }
        public Command RefreshCommand { get; }
        public Command AddItemCommand { get; }
        
        public Command<Person> ItemTapped { get; }

        public PersonViewModel()
        {
            Title = "Personas";
            Items = new ObservableCollection<Person>();
            LoadPersonCommand = new Command(async () => await ExecuteLoadPersonCommand());

            RefreshCommand = new Command(async () => await ExecuteLoadPersonCommand(true));
            
            ItemTapped = new Command<Person>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            
        }

        async Task ExecuteLoadPersonCommand(bool loadAPI = false)
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var items = await PersonStore.GetItemsAsync(true, loadAPI);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Person SelectedItem
        {
            get => _selectItem;
            set
            {
                SetProperty(ref _selectItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewPersonPage));
        }  

        async void OnItemSelected(Person item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(PersonDetailPage)}?{nameof(PersonDetailViewModel.ItemId)}={item.Id}");
        }
    }
}
