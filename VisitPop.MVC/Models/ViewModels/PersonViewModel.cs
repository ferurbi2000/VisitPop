using System.Collections.Generic;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class PersonViewModel : EntitiesActionsViewModel
    {
        public PersonDto Person { get; set; }
        public IEnumerable<PersonTypeDto> PersonTypes { get; set; }
        public IEnumerable<CompanyDto> Companies { get; set; }
    }

    public static class PersonViewModelFactory
    {
        public static PersonViewModel Details(PersonDto person, string returnUrl, IEnumerable<PersonTypeDto> personTypes, IEnumerable<CompanyDto> companies)
        {
            return new PersonViewModel
            {
                Person = person,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl,
                PersonTypes = personTypes,
                Companies = companies
            };
        }

        public static PersonViewModel Create(PersonDto person, string returnUrl, IEnumerable<PersonTypeDto> personTypes, IEnumerable<CompanyDto> companies)
        {
            return new PersonViewModel
            {
                Person = person,
                ReturnUrl = returnUrl,
                PersonTypes = personTypes,
                Companies = companies
            };
        }
        public static PersonViewModel Edit(PersonDto person, string returnUrl, IEnumerable<PersonTypeDto> personTypes, IEnumerable<CompanyDto> companies)
        {
            return new PersonViewModel
            {
                Person = person,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl,
                PersonTypes = personTypes,
                Companies = companies
            };
        }

        public static PersonViewModel Delete(PersonDto person, string returnUrl, IEnumerable<PersonTypeDto> personTypes, IEnumerable<CompanyDto> companies)
        {
            return new PersonViewModel
            {
                Person = person,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl,
                PersonTypes = personTypes,
                Companies = companies
            };
        }
    }
}
