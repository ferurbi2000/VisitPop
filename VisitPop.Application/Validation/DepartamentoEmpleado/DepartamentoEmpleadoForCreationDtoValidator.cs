using VisitPop.Application.Dtos.DepartamentoEmpleado;

namespace VisitPop.Application.Validation.DepartamentoEmpleado
{
    public class DepartamentoEmpleadoForCreationDtoValidator: DepartamentoEmpleadoForManipulationDtoValidator<DepartamentoEmpleadoForCreationDto>
    {
        public DepartamentoEmpleadoForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
