using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class RegisterControlViewModel : EntitiesActionsViewModel
    {
        public RegisterControlDto RegisterControl { get; set; }
    }

    public static class RegisterControlViewModelFactory
    {
        public static RegisterControlViewModel Details(RegisterControlDto registerControl, string returnUrl)
        {
            return new RegisterControlViewModel
            {
                RegisterControl = registerControl,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static RegisterControlViewModel Create(RegisterControlDto registerControl, string returnUrl)
        {
            return new RegisterControlViewModel
            {
                RegisterControl = registerControl,
                ReturnUrl = returnUrl
            };
        }
        public static RegisterControlViewModel Edit(RegisterControlDto registerControl, string returnUrl)
        {
            return new RegisterControlViewModel
            {
                RegisterControl = registerControl,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static RegisterControlViewModel Delete(RegisterControlDto registerControl, string returnUrl)
        {
            return new RegisterControlViewModel
            {
                RegisterControl = registerControl,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
