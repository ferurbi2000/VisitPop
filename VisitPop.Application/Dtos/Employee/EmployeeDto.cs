using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Validation;
using VisitPop.Domain.Common;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.Employee
{
    public class EmployeeDto: AuditableEntity
    {
        //public int Id { get; set; }

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

        [Required(ErrorMessage = "You must select a Department")]        
        public int? EmployeeDepartmentId { get; set; }

        [StringLength(VisitEntityConstants.MAX_EMAIL_LENGTH)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        //public EmployeeDepartmentDto EmployeeDepartment { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{LastName}, {FirstName}";


        // add-on property marker - Do Not Delete This Comment
    }
}
