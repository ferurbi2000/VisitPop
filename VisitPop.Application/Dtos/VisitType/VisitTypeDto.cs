using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.VisitType
{
    public class VisitTypeDto : AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "You must enter the Visit Type Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name = "Visit Type")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
