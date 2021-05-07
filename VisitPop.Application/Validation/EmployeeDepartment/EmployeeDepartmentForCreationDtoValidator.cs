using VisitPop.Application.Dtos.EmployeeDepartment;

namespace VisitPop.Application.Validation.EmployeeDepartment
{
    public class EmployeeDepartmentForCreationDtoValidator : EmployeeDepartmentForManipulationDtoValidator<EmployeeDepartmentForCreationDto>
    {
        public EmployeeDepartmentForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
