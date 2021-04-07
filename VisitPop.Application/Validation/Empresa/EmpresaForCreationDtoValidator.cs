using VisitPop.Application.Dtos.Empresa;

namespace VisitPop.Application.Validation.Empresa
{
    public class EmpresaForCreationDtoValidator : EmpresaForManipulationDtoValidator<EmpresaForCreationDto>
    {
        public EmpresaForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
