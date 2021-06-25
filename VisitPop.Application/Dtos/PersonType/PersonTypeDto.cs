using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.PersonType
{
    public class PersonTypeDto : AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "You must enter the Person Type Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name = "Person Type")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
