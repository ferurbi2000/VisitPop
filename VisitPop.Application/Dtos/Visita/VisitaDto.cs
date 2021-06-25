using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Dtos.VisitType;

namespace VisitPop.Application.Dtos.Visita
{
    public class VisitaDto
    {
        public int Id { get; set; }
        public string Motivo { get; set; }
        public int VisitTypeId { get; set; }
        public string Empresa { get; set; }
        public int EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public bool IsCitaPrevia { get; set; }
        public DateTime? FechaCita { get; set; }
        public int PuntoControlId { get; set; }
        public int VisitStateId { get; set; }
        public VisitTypeDto VisitType { get; set; }
        public EmployeeDto Employee { get; set; }
        public OfficeDto Office { get; set; }
        public RegisterControlDto RegisterControl { get; set; }
        public VisitStateDto VisitState { get; set; }

    }
}
