using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("Empleado")]
    public class Empleado : AuditableEntity
    {
        [Required(ErrorMessage = "Debe ingresar los nombres")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter =true, CanSort =true)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Debe ingresar los apellidos")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter =true, CanSort =true)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(VisitEntityConstants.MAX_DOC_ID_LENGTH)]
        [Sieve(CanFilter =true, CanSort =true)]
        public string Identidad { get; set; }

        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Telefono { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int EmployeeDepartmentId { get; set; }

        [StringLength(VisitEntityConstants.MAX_EMAIL_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Email { get; set; }

        [ForeignKey("EmployeeDepartmentId")]
        public EmployeeDepartment EmployeeDepartments { get; set; }
    }
}
