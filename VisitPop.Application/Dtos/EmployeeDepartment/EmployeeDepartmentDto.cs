using System.ComponentModel.DataAnnotations;
using VisitPop.Domain.Constants;

namespace VisitPop.Application.Dtos.EmployeeDepartment
{
    public class EmployeeDepartmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter the Department Name")]
        [StringLength(VisitEntityConstants.MAX_NAMES_LENGTH)]
        [Display(Name ="Department")]
        public string Name { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
