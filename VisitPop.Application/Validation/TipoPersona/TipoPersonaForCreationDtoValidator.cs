using VisitPop.Application.Dtos.TipoPersona;

namespace VisitPop.Application.Validation.TipoPersona
{
    public class TipoPersonaForCreationDtoValidator : TipoPersonaForManipulationDtoValidator<TipoPersonaForCreationDto>
    {
        public TipoPersonaForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
