using FluentValidation;
using VisitPop.Application.Dtos.Employee;

namespace VisitPop.Application.Validation.Employee
{
    public class EmployeeForCreationDtoValidator: EmployeeForManipulationDtoValidator<EmployeeForCreationDto>
    {
        public EmployeeForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
            //
            
        }
    }
}
