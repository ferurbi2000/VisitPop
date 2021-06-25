using VisitPop.Application.Dtos.Company;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class CompanyViewModel : EntitiesActionsViewModel
    {
        public CompanyDto Company { get; set; }
    }

    public static class CompanyViewModelFactory
    {
        public static CompanyViewModel Details(CompanyDto company, string returnUrl)
        {
            return new CompanyViewModel
            {
                Company = company,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static CompanyViewModel Create(CompanyDto company, string returnUrl)
        {
            return new CompanyViewModel
            {
                Company = company,
                ReturnUrl = returnUrl
            };
        }
        public static CompanyViewModel Edit(CompanyDto company, string returnUrl)
        {
            return new CompanyViewModel
            {
                Company = company,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static CompanyViewModel Delete(CompanyDto company, string returnUrl)
        {
            return new CompanyViewModel
            {
                Company = company,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
