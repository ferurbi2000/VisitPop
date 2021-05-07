using FluentValidation;
using VisitPop.Application.Dtos.EmployeeDepartment;

namespace VisitPop.Application.Validation.EmployeeDepartment
{
    public class EmployeeDepartmentForManipulationDtoValidator<T>
        : AbstractValidator<T> where T : EmployeeDepartmentForManipulationDto
    {
        public EmployeeDepartmentForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}