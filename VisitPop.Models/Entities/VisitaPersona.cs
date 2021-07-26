using Sieve.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("VisitaPersona")]
    public class VisitaPersona : AuditableEntity
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = false)]
        public int VisitaId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = false)]
        public int PersonaId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = false)]
        public int TipoVehiculoId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        public string Placa { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? FechaIngresa { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? FechaSalida { get; set; }

        [ForeignKey("VisitaId")]
        public Visit Visita { get; set; }

        [ForeignKey("PersonaId")]
        public Person Persona { get; set; }

        [ForeignKey("TipoVehiculoId")]
        public VehicleType TipoVehiculo { get; set; }

        // add-on property marker - Do Not Delete This Comment

        //Pertenencias

    }
}
