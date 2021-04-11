using VisitPop.Application.Dtos.VisitaPersona;

namespace VisitPop.Application.Validation.VisitaPersona
{
    public class VisitaPersonaForUpdateDtoValidator : VisitaPersonaForManipulationDtoValidator<VisitaPersonaForUpdateDto>
    {
        public VisitaPersonaForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
