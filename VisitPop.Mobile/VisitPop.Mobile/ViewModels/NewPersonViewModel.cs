using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VisitPop.Mobile.Models;
using Xamarin.Forms;

namespace VisitPop.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class NewPersonViewModel: BaseViewModel
    {
        private string itemId;
        private string nombres;
        private string apellidos;
        private string docidentidad;

        public NewPersonViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(nombres)
                && !String.IsNullOrWhiteSpace(apellidos);
        }

        public string Id
        {
            get;
            set;
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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Person newItem = new Person()
            {
                //Id = new Random().Next(100),
                Nombres = Nombres,
                Apellidos = Apellidos,
                DocIdentidad= DocIdentidad
            };

            await PersonStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
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
    }
}
