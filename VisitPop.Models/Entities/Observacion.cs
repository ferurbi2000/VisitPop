using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("Observacion")]
    public class Observacion : AuditableEntity
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int VisitaId { get; set; }

        [Required]
        [StringLength(VisitEntityConstants.MAX_NOTES_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Nota { get; set; }

        [ForeignKey("VisitaId")]
        public Visita Visita { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
