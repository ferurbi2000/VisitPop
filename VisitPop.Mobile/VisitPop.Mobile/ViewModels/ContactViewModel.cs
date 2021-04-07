using VisitPop.Mobile.Models;

namespace VisitPop.Mobile.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {        
        public ContactViewModel() { }

        public ContactViewModel(Person contact)
        {
            Id = contact.Id;
            _nombres = contact.Nombres;            
            _apellidos = contact.Apellidos;            
            DocIdentidad = contact.DocIdentidad;
            Telefono1 = contact.Telefono1;           
        }

        private int _id;
        public int Id
        {
            get => _id;
            set { SetProperty(ref _id, value); }
        }

        private string _nombres;
        public string Nombres
        {
            get => _nombres;
            set
            {
                SetProperty(ref _nombres, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _apellidos;
        public string Apellidos
        {
            get => _apellidos;
            set
            {
                SetProperty(ref _apellidos, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _docIdentidad;
        public string DocIdentidad
        {
            get => _docIdentidad;
            set { SetProperty(ref _docIdentidad, value); }
        }

        private string _telefono1;
        public string Telefono1
        {
            get => _telefono1; 
            set { SetProperty(ref _telefono1, value); }
        }

        public string FullName { get => $"{Nombres} {Apellidos}"; }

    }
}
