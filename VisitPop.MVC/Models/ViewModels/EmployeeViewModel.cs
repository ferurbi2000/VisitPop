using System.Collections;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Domain.Entities;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class EmployeeViewModel : EntitiesActionsViewModel
    {
        public EmployeeDto Employee { get; set; }
        public IEnumerable<EmployeeDepartmentDto> EmployeeDepartment { get; set; }
    }

    public static class EmployeeViewModelFactory
    {
        public static EmployeeViewModel Details(EmployeeDto employee, string returnUrl, IEnumerable<EmployeeDepartmentDto> employeeDepartments)
        {
            return new EmployeeViewModel
            {
                Employee = employee,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl,
                EmployeeDepartment = employeeDepartments
            };
        }

        public static EmployeeViewModel Create(EmployeeDto employee, string returnUrl, IEnumerable<EmployeeDepartmentDto> employeeDepartments)
        {
            return new EmployeeViewModel
            {
                Employee = employee,
                ReturnUrl = returnUrl,
                EmployeeDepartment = employeeDepartments
            };
        }
        public static EmployeeViewModel Edit(EmployeeDto employee, string returnUrl, IEnumerable<EmployeeDepartmentDto> employeeDepartments)
        {
            return new EmployeeViewModel
            {
                Employee = employee,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ReturnUrl = returnUrl,                
                EmployeeDepartment = employeeDepartments
            };
        }

        public static EmployeeViewModel Delete(EmployeeDto employee, string returnUrl, IEnumerable<EmployeeDepartmentDto> employeeDepartments)
        {
            return new EmployeeViewModel
            {
                Employee = employee,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl,
                EmployeeDepartment = employeeDepartments
            };
        }
    }
}
