using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;

namespace VisitPop.Domain.Entities
{
    [Table("Employee")]
    public class Employee : AuditableEntity
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string DocId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PhoneNumber { get; set; }
        
        [Sieve(CanFilter = true, CanSort = true)]
        public int EmployeeDepartmentId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string EmailAddress { get; set; }

        [ForeignKey("EmployeeDepartmentId")]
        public EmployeeDepartment EmployeeDepartments { get; set; }        
    }
}
