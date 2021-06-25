using FluentValidation;
using VisitPop.Application.Dtos.Employee;

namespace VisitPop.Application.Validation.Employee
{
    public class EmployeeForManipulationDtoValidator<T> : AbstractValidator<T> where T : EmployeeForManipulationDto
    {
        public EmployeeForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/            
        }
    }
}
