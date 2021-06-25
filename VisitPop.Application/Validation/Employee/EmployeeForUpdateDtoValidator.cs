using VisitPop.Application.Dtos.Employee;
using FluentValidation;

namespace VisitPop.Application.Validation.Employee
{
    public class EmployeeForUpdateDtoValidator : EmployeeForManipulationDtoValidator<EmployeeForUpdateDto>
    {
        public EmployeeForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
