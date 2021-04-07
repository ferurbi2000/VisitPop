using VisitPop.Application.Dtos.DepartamentoEmpleado;

namespace VisitPop.Application.Validation.DepartamentoEmpleado
{
    public class DepartamentoEmpleadoForUpdateDtoValidator: DepartamentoEmpleadoForManipulationDtoValidator<DepartamentoEmpleadoForUpdateDto>
    {
        public DepartamentoEmpleadoForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
