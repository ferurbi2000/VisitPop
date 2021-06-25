using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.Company
{
    public class CompanyDto: AuditableEntity
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = "You must enter the Company Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name ="Company")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
