using VisitPop.Application.Dtos.VehicleType;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class VehicleTypeViewModel : EntitiesActionsViewModel
    {
        public VehicleTypeDto VehicleType { get; set; }
    }

    public static class VehicleTypeViewModelFactory
    {
        public static VehicleTypeViewModel Details(VehicleTypeDto vehicleType, string returnUrl)
        {
            return new VehicleTypeViewModel
            {
                VehicleType = vehicleType,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static VehicleTypeViewModel Create(VehicleTypeDto vehicleType, string returnUrl)
        {
            return new VehicleTypeViewModel
            {
                VehicleType = vehicleType,
                ReturnUrl = returnUrl
            };
        }
        public static VehicleTypeViewModel Edit(VehicleTypeDto vehicleType, string returnUrl)
        {
            return new VehicleTypeViewModel
            {
                VehicleType = vehicleType,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static VehicleTypeViewModel Delete(VehicleTypeDto vehicleType, string returnUrl)
        {
            return new VehicleTypeViewModel
            {
                VehicleType = vehicleType,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }
}
