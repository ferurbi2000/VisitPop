using VisitPop.Application.Dtos.TipoVisita;

namespace VisitPop.Application.Validation.TipoVisita
{
    public class TipoVisitaForCreationDtoValidator : TipoVisitaForManipulationDtoValidator<TipoVisitaForCreationDto>
    {
        public TipoVisitaForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
