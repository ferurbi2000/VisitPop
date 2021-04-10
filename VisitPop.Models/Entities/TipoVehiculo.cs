using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("TipoVehiculo")]
    public class TipoVehiculo : AuditableEntity
    {
        [Required]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Nombre { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
