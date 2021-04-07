using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Domain.Entities
{
    public class Category: AuditableEntity
    {
        [Required(ErrorMessage = "Debe ingresar los nombres")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }

        [StringLength(VisitEntityConstants.MAX_DESCRIPTION_LENGTH)]       
        public string Description { get; set; }
    }
}
