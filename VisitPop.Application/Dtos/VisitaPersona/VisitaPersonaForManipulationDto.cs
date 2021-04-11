using System;

namespace VisitPop.Application.Dtos.VisitaPersona
{
    public abstract class VisitaPersonaForManipulationDto
    {
        public int VisitaId { get; set; }
        public int PersonaId { get; set; }
        public int TipoVehiculoId { get; set; }
        public string Placa { get; set; }
        public DateTime? FechaIngresa { get; set; }
        public DateTime? FechaSalida { get; set; }


        // add-on property marker - Do Not Delete This Comment
    }
}
