using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.VisitState
{
    public class VisitStateDto: AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "You must enter the Visit State Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name = "Visit State")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comments
    }
}
