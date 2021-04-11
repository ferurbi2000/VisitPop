using VisitPop.Application.Dtos.TipoVisita;

namespace VisitPop.Application.Validation.TipoVisita
{
    public class TipoVisitaForUpdateDtoValidator : TipoVisitaForManipulationDtoValidator<TipoVisitaForUpdateDto>
    {
        public TipoVisitaForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
