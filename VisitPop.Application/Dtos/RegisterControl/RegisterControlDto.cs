using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.RegisterControl
{
    public class RegisterControlDto : AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage ="You must enter the Name Register Control")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name ="Register Control Name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="You must enter the Location of the Register Control")]
        [StringLength(VisitEntityConstants.MAX_ADDRESS_LENGTH)]
        public string Location { get; set; }

        [StringLength(VisitEntityConstants.MAX_NOTES_LENGTH)]
        public string Description { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
