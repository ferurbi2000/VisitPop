using System;

namespace VisitPop.Application.Dtos.Visit
{
    public abstract class VisitForManipulationDto
    {
        public string Reason { get; set; }
        public int VisitTypeId { get; set; }
        public string Company { get; set; }
        public int EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public bool IsAppointment { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int RegisterControlId { get; set; }
        public int VisitStateId { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
