using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VisitPop.Mobile.Models;
using VisitPop.Mobile.Views;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class PersonDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string id;
        private string nombres;
        private string apellidos;
        private string docidentidad;

        public Command<string> EditItemCommand { get; }

        public PersonDetailViewModel()
        {
            EditItemCommand = new Command<string>(OnEditItem);
        }

        //public string Id { get; set; }

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Nombres
        {
            get => nombres;
            set => SetProperty(ref nombres, value);
        }

        public string Apellidos
        {
            get => apellidos;
            set => SetProperty(ref apellidos, value);
        }

        public string DocIdentidad
        {
            get => docidentidad;
            set => SetProperty(ref docidentidad, value);
        }

        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;                
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await PersonStore.GetItemAsync(itemId);
                Id = item.Id.ToString();                
                Nombres = item.Nombres;
                Apellidos = item.Apellidos;
                DocIdentidad = item.DocIdentidad;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load Item");
            }
        }

        private async void OnEditItem(string id)
        {
            //await Shell.Current.GoToAsync(nameof(NewPersonPage));
            //await Shell.Current.GoToAsync($"{nameof(NewPersonPage)}?{nameof(PersonDetailViewModel.ItemId)}");
            //await Shell.Current.GoToAsync($"{nameof(NewPersonPage)}?{nameof(PersonDetailViewModel.ItemId)}");
            await Shell.Current.GoToAsync($"{nameof(NewPersonPage)}?{-134839010}");
        }
    }
}
