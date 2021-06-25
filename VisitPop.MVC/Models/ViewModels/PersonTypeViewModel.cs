using VisitPop.Application.Dtos.PersonType;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class PersonTypeViewModel : EntitiesActionsViewModel
    {
        public PersonTypeDto PersonType { get; set; }
    }

    public static class PersonTypeViewModelFactory
    {
        public static PersonTypeViewModel Details(PersonTypeDto personType, string returnUrl)
        {
            return new PersonTypeViewModel
            {
                PersonType = personType,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static PersonTypeViewModel Create(PersonTypeDto personType, string returnUrl)
        {
            return new PersonTypeViewModel
            {
                PersonType = personType,
                ReturnUrl = returnUrl
            };
        }
        public static PersonTypeViewModel Edit(PersonTypeDto personType, string returnUrl)
        {
            return new PersonTypeViewModel
            {
                PersonType = personType,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static PersonTypeViewModel Delete(PersonTypeDto personType, string returnUrl)
        {
            return new PersonTypeViewModel
            {
                PersonType = personType,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
