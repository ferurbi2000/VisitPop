using FluentValidation;
using VisitPop.Application.Dtos.DepartamentoEmpleado;

namespace VisitPop.Application.Validation.DepartamentoEmpleado
{
    public class DepartamentoEmpleadoForManipulationDtoValidator<T>
        : AbstractValidator<T> where T: DepartamentoEmpleadoForManipulationDto 
    {
        public DepartamentoEmpleadoForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}