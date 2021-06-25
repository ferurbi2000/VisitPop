using VisitPop.Application.Dtos.VisitType;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class VisitTypeViewModel : EntitiesActionsViewModel
    {
        public VisitTypeDto VisitType { get; set; }
    }

    public static class VisitTypeViewModelFactory
    {
        public static VisitTypeViewModel Details(VisitTypeDto visitType, string returnUrl)
        {
            return new VisitTypeViewModel
            {
                VisitType = visitType,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static VisitTypeViewModel Create(VisitTypeDto visitType, string returnUrl)
        {
            return new VisitTypeViewModel
            {
                VisitType = visitType,
                ReturnUrl = returnUrl
            };
        }
        public static VisitTypeViewModel Edit(VisitTypeDto visitType, string returnUrl)
        {
            return new VisitTypeViewModel
            {
                VisitType = visitType,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static VisitTypeViewModel Delete(VisitTypeDto visitType, string returnUrl)
        {
            return new VisitTypeViewModel
            {
                VisitType = visitType,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
