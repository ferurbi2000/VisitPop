using VisitPop.Application.Dtos.Office;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class OfficeViewModel : EntitiesActionsViewModel
    {
        public OfficeDto Office { get; set; }
    }

    public static class OfficeViewModelFactory
    {
        public static OfficeViewModel Details(OfficeDto office, string returnUrl)
        {
            return new OfficeViewModel
            {
                Office = office,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static OfficeViewModel Create(OfficeDto office, string returnUrl)
        {
            return new OfficeViewModel
            {
                Office = office,
                ReturnUrl = returnUrl
            };
        }
        public static OfficeViewModel Edit(OfficeDto office, string returnUrl)
        {
            return new OfficeViewModel
            {
                Office = office,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static OfficeViewModel Delete(OfficeDto office, string returnUrl)
        {
            return new OfficeViewModel
            {
                Office = office,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
