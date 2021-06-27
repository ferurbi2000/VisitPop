using System;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.Application.Dtos.Visita;

namespace VisitPop.Application.Dtos.VisitaPersona
{
    public class VisitaPersonaDto
    {
        public int Id { get; set; }
        public int VisitaId { get; set; }
        public int PersonaId { get; set; }
        public int TipoVehiculoId { get; set; }
        public string Placa { get; set; }
        public DateTime? FechaIngresa { get; set; }
        public DateTime? FechaSalida { get; set; }
        public VisitaDto Visita { get; set; }
        public PersonDto Person { get; set; }
        public VehicleTypeDto VehicleType { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
