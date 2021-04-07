using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Domain.Common;

namespace VisitPop.Domain.Entities
{
    public class Visita: AuditableEntity
    {
        public string Motivo { get; set; }
    }
}
