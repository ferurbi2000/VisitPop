using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    [Table("PuntoControl")]
    public class PuntoControl : AuditableEntity
    {
        [Required]
        [Sieve(CanFilter =true, CanSort =true)]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        public string Nombre { get; set; }
        
        [Required]
        [Sieve(CanFilter =true, CanSort =true)]
        [StringLength(VisitEntityConstants.MAX_ADDRESS_LENGTH)]
        public string Ubicacion { get; set; }

        [StringLength(VisitEntityConstants.MAX_NOTES_LENGTH)]
        public string Descripcion { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
