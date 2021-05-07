using VisitPop.Application.Dtos.EmployeeDepartment;

namespace VisitPop.Application.Validation.EmployeeDepartment
{
    public class EmployeeDepartmentForUpdateDtoValidator : EmployeeDepartmentForManipulationDtoValidator<EmployeeDepartmentForUpdateDto>
    {
        public EmployeeDepartmentForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
