using VisitPop.Application.Dtos.TipoPersona;

namespace VisitPop.Application.Validation.TipoPersona
{
    public class TipoPersonaForUpdateDtoValidator : TipoPersonaForManipulationDtoValidator<TipoPersonaForUpdatedDto>
    {
        public TipoPersonaForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
