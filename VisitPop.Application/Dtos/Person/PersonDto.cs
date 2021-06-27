using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.Person
{
    public class PersonDto : AuditableEntity
    {

        [Required(ErrorMessage = "You must enter the First Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter the Last Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(VisitEntityConstants.MAX_DOC_ID_LENGTH)]
        [Display(Name = "Document ID")]
        public string DocId { get; set; }

        [StringLength(VisitEntityConstants.MAX_PHONE_LENGTH)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "You must select a Person Type")]
        public int? PersonTypeId { get; set; }

        [Required(ErrorMessage = "You must select a Company")]
        public int? CompanyId { get; set; }

        [StringLength(VisitEntityConstants.MAX_EMAIL_LENGTH)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        //public PersonTypeDto PersonType { get; set; }
        //public CompanyDto Empresa { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
