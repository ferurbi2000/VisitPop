using VisitPop.Application.Dtos.VisitState;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class VisitStateViewModel : EntitiesActionsViewModel
    {
        public VisitStateDto VisitState { get; set; }
    }

    public static class VisitStateViewModelFactory
    {
        public static VisitStateViewModel Details(VisitStateDto visitState, string returnUrl)
        {
            return new VisitStateViewModel
            {
                VisitState = visitState,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static VisitStateViewModel Create(VisitStateDto visitState, string returnUrl)
        {
            return new VisitStateViewModel
            {
                VisitState = visitState,
                ReturnUrl = returnUrl
            };
        }
        public static VisitStateViewModel Edit(VisitStateDto visitState, string returnUrl)
        {
            return new VisitStateViewModel
            {
                VisitState = visitState,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static VisitStateViewModel Delete(VisitStateDto visitState, string returnUrl)
        {
            return new VisitStateViewModel
            {
                VisitState = visitState,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
