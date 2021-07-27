using System.Collections.Generic;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Application.Dtos.VisitPerson;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    // TODO: Convertir las entidades ViewModel Factories a Clases Genericas
    public class VisitViewModel : EntitiesActionsViewModel
    {
        public VisitDto Visit { get; set; }

        public IEnumerable<VisitTypeDto> VisitTypes { get; set; }
        public IEnumerable<EmployeeDto> Employees { get; set; }
        public IEnumerable<OfficeDto> Offices { get; set; }
        public IEnumerable<RegisterControlDto> RegisterControls { get; set; }
        public IEnumerable<VisitStateDto> VisitStates { get; set; }

        public IEnumerable<VisitPersonDto> VisitPersons { get; set; }

    }

    public static class VisitViewModelFactory
    {
        public static VisitViewModel Details(VisitDto visit, string returnUrl,
            IEnumerable<VisitTypeDto> visitTypes,
            IEnumerable<EmployeeDto> employees,
            IEnumerable<OfficeDto> offices,
            IEnumerable<RegisterControlDto> registerControls,
            IEnumerable<VisitStateDto> visitStates,
            IEnumerable<VisitPersonDto> visitPersons)
        {
            return new VisitViewModel
            {
                Visit = visit,
                Action = "Details",
                ReadOnly = true,
                Theme = "info",
                ShowAction = false,
                ReturnUrl = returnUrl,
                VisitTypes = visitTypes,
                Employees = employees,
                Offices = offices,
                RegisterControls = registerControls,
                VisitStates = visitStates,
                VisitPersons = visitPersons
            };
        }

        public static VisitViewModel Create(VisitDto visit, string returnUrl,
            IEnumerable<VisitTypeDto> visitTypes,
            IEnumerable<EmployeeDto> employees,
            IEnumerable<OfficeDto> offices,
            IEnumerable<RegisterControlDto> registerControls,
            IEnumerable<VisitStateDto> visitStates)
        {
            return new VisitViewModel
            {
                Visit = visit,
                ReturnUrl = returnUrl,
                VisitTypes = visitTypes,
                Employees = employees,
                Offices = offices,
                RegisterControls = registerControls,
                VisitStates = visitStates
            };
        }
        public static VisitViewModel Edit(VisitDto visit, string returnUrl,
            IEnumerable<VisitTypeDto> visitTypes,
            IEnumerable<EmployeeDto> employees,
            IEnumerable<OfficeDto> offices,
            IEnumerable<RegisterControlDto> registerControls,
            IEnumerable<VisitStateDto> visitStates,
            IEnumerable<VisitPersonDto> visitPersons)
        {
            return new VisitViewModel
            {
                Visit = visit,
                Theme = "warning",
                Action = "Edit",
                ShowCreateNewAction = true,
                ShowCreateNewDetails = true,
                ReturnUrl = returnUrl,
                VisitTypes = visitTypes,
                Employees = employees,
                Offices = offices,
                RegisterControls = registerControls,
                VisitStates = visitStates,
                VisitPersons = visitPersons
            };
        }

        public static VisitViewModel Delete(VisitDto visit, string returnUrl,
            IEnumerable<VisitTypeDto> visitTypes,
            IEnumerable<EmployeeDto> employees,
            IEnumerable<OfficeDto> offices,
            IEnumerable<RegisterControlDto> registerControls,
            IEnumerable<VisitStateDto> visitStates,
            IEnumerable<VisitPersonDto> visitPersons)
        {
            return new VisitViewModel
            {
                Visit = visit,
                Action = "Delete",
                ReadOnly = true,
                Theme = "danger",
                ReturnUrl = returnUrl,
                VisitTypes = visitTypes,
                Employees = employees,
                Offices = offices,
                RegisterControls = registerControls,
                VisitStates = visitStates,
                VisitPersons = visitPersons
            };
        }
    }
}
