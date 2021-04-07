using FluentValidation;
using VisitPop.Application.Dtos.Empresa;

namespace VisitPop.Application.Validation.Empresa
{
    public class EmpresaForManipulationDtoValidator<T> : AbstractValidator<T> where T : EmpresaForManipulationDto
    {
        public EmpresaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
