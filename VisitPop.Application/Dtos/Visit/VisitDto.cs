using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.VisitPerson;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.Visit
{
    public class VisitDto : AuditableEntity
    {
        [Required(ErrorMessage = "You must enter the reason of this Visit")]
        [StringLength(VisitEntityConstants.MAX_NOTES_LENGTH)]
        public string Reason { get; set; }

        [Required(ErrorMessage = "You must select a Visit Type")]
        public int? VisitTypeId { get; set; }

        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        public string Company { get; set; }

        [Required(ErrorMessage = "You must select a Employee")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "You must select a Office")]
        public int? OfficeId { get; set; }

        [Display(Name ="Is Appointment")]
        public bool IsAppointment { get; set; }

        [Display(Name = "Appointment Date")]
        //[DataType(DataType.Date)]
        public DateTime? AppointmentDate { get; set; }

        [Required(ErrorMessage = "You must select a Register Control")]
        public int? RegisterControlId { get; set; }

        [Required(ErrorMessage = "You must select a Visit State")]
        public int? VisitStateId { get; set; }

        public VisitTypeDto VisitType { get; set; }
        public EmployeeDto Employee { get; set; }
        //public OfficeDto Office { get; set; }
        //public RegisterControlDto RegisterControl { get; set; }
        public VisitStateDto VisitState { get; set; }        

    }
}
