using VisitPop.Application.Dtos.Empresa;

namespace VisitPop.Application.Validation.Empresa
{
    public class EmpresaForUpdateDtoValidator : EmpresaForManipulationDtoValidator<EmpresaForUpdateDto>
    {
        public EmpresaForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
