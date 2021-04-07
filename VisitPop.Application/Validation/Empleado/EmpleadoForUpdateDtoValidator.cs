using VisitPop.Application.Dtos.Empleado;
using FluentValidation;

namespace VisitPop.Application.Validation.Empleado
{
    public class EmpleadoForUpdateDtoValidator : EmpleadoForManipulationDtoValidator<EmpleadoForUpdateDto>
    {
        public EmpleadoForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
