using Sieve.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;

namespace VisitPop.Domain.Entities
{
    [Table("Visit")]
    public class Visit : AuditableEntity
    {

        [Sieve(CanFilter = true, CanSort = true)]
        public string Reason { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int VisitTypeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Company { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int EmployeeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int OfficeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public bool IsAppointment { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? AppointmentDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int RegisterControlId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int VisitStateId { get; set; }

        [ForeignKey("VisitTypeId")]
        public VisitType VisitType { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [ForeignKey("OfficeId")]
        public Office Office { get; set; }

        [ForeignKey("RegisterControlId")]
        public RegisterControl RegisterControl { get; set; }

        [ForeignKey("VisitStateId")]
        public VisitState VisitState { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
