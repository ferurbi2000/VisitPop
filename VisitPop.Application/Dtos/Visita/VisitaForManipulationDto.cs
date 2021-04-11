using System;

namespace VisitPop.Application.Dtos.Visita
{
    public abstract class VisitaForManipulationDto
    {
        public string Motivo { get; set; }
        public int TipoVisitaId { get; set; }
        public string Empresa { get; set; }
        public int EmpleadoId { get; set; }
        public int OficinaId { get; set; }
        public bool IsCitaPrevia { get; set; }
        public DateTime? FechaCita { get; set; }
        public int PuntoControlId { get; set; }
        public int EstadoVisitaId { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
