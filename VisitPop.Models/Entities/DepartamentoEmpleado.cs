using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("DepartamentoEmpleado")]
    public class DepartamentoEmpleado : AuditableEntity
    {

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Nombre { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
