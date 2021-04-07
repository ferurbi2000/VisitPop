using VisitPop.Application.Dtos.Empleado;

namespace VisitPop.Application.Validation.Empleado
{
    public class EmpleadoForCreationDtoValidator: EmpleadoForManipulationDtoValidator<EmpleadoForCreationDto>
    {
        public EmpleadoForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
