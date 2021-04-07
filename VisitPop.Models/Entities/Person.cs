using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    public class Person: AuditableEntity
    {
        [Required(ErrorMessage ="Debe ingresar los nombres")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter =true, CanSort =true)]
        public string Nombres { get; set; }
        
        [Required(ErrorMessage ="Debe ingresar los apellidos")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Apellidos { get; set; }

        [Required]
        [Display(Name ="Cedula")]
        [StringLength(VisitEntityConstants.MAX_DOC_ID_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string DocIdentidad { get; set; }

        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Telefono1 { get; set; }

        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        public string Telefono2 { get; set; }

        [StringLength(VisitEntityConstants.MAX_EMAIL_LENGTH)]
        public string Email { get; set; }

        [StringLength(VisitEntityConstants.MAX_NOTES_LENGTH)]
        public string Observacion { get; set; }
    }
}
