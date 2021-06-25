using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class EmployeeDepartmentViewModel : EntitiesActionsViewModel
    {
        public EmployeeDepartmentDto EmployeeDepartment { get; set; }

    }

    public static class EmployeeDepartmentViewModelFactory
    {
        public static EmployeeDepartmentViewModel Details(EmployeeDepartmentDto employeeDepartment, string returnUrl)
        {
            return new EmployeeDepartmentViewModel
            {
                EmployeeDepartment = employeeDepartment,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl
            };
        }

        public static EmployeeDepartmentViewModel Create(EmployeeDepartmentDto employeeDepartment, string returnUrl)
        {
            return new EmployeeDepartmentViewModel
            {
                EmployeeDepartment = employeeDepartment,
                ReturnUrl = returnUrl
            };
        }
        public static EmployeeDepartmentViewModel Edit(EmployeeDepartmentDto employeeDepartment, string returnUrl)
        {
            return new EmployeeDepartmentViewModel
            {
                EmployeeDepartment = employeeDepartment,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl
            };
        }

        public static EmployeeDepartmentViewModel Delete(EmployeeDepartmentDto employeeDepartment, string returnUrl)
        {
            return new EmployeeDepartmentViewModel
            {
                EmployeeDepartment = employeeDepartment,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl
            };
        }
    }

}
