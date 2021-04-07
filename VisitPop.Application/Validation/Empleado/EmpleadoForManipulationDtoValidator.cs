using FluentValidation;
using VisitPop.Application.Dtos.Empleado;

namespace VisitPop.Application.Validation.Empleado
{
    public class EmpleadoForManipulationDtoValidator<T> : AbstractValidator<T> where T : EmpleadoForManipulationDto
    {
        public EmpleadoForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
