using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.Office
{
    public class OfficeDto : AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "You must enter the Office Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name = "Office Name")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
