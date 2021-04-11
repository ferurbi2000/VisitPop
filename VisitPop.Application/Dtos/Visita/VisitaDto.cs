using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Empleado;
using VisitPop.Application.Dtos.EstadoVisita;
using VisitPop.Application.Dtos.Oficina;
using VisitPop.Application.Dtos.PuntoControl;
using VisitPop.Application.Dtos.TipoVisita;

namespace VisitPop.Application.Dtos.Visita
{
    public class VisitaDto
    {
        public int Id { get; set; }
        public string Motivo { get; set; }
        public int TipoVisitaId { get; set; }
        public string Empresa { get; set; }
        public int EmpleadoId { get; set; }
        public int OficinaId { get; set; }
        public bool IsCitaPrevia { get; set; }
        public DateTime? FechaCita { get; set; }
        public int PuntoControlId { get; set; }
        public int EstadoVisitaId { get; set; }
        public TipoVisitaDto TipoVisita { get; set; }
        public EmpleadoDto Empleado { get; set; }
        public OficinaDto Oficina { get; set; }
        public PuntoControlDto PuntoControl { get; set; }
        public EstadoVisitaDto EstadoVisita { get; set; }

    }
}
