using Sieve.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("Visita")]
    public class Visita : AuditableEntity
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_NOTES_LENGTH)]
        public string Motivo { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TipoVisitaId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        public string Empresa { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int EmpleadoId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int OficinaId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public bool IsCitaPrevia { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? FechaCita { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int PuntoControlId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int EstadoVisitaId { get; set; }

        [ForeignKey("TipoVisitaId")]
        public VisitType TipoVisita { get; set; }

        [ForeignKey("EmpleadoId")]
        public Employee Empleado { get; set; }

        [ForeignKey("OficinaId")]
        public Office Oficina { get; set; }

        [ForeignKey("PuntoControlId")]
        public RegisterControl PuntoControl { get; set; }

        [ForeignKey("EstadoVisitaId")]
        public VisitState EstadoVisita { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
