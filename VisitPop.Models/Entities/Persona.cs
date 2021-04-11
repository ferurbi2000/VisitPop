using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("Persona")]
    public class Persona : AuditableEntity
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        public string Nombres { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        public string Apellidos { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_DOC_ID_LENGTH)]
        public string Identidad { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        public string Telefono { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TipoPersonaId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int EmpresaId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_EMAIL_LENGTH)]
        public string Email { get; set; }


        [ForeignKey("TipoPersonaId")]
        public TipoPersona TipoPersona { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        // add-on property marker - Do Not Delete This Comment

    }
}
