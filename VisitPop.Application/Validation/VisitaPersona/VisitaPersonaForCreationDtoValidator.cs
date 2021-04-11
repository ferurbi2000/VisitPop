using VisitPop.Application.Dtos.VisitaPersona;

namespace VisitPop.Application.Validation.VisitaPersona
{
    public class VisitaPersonaForCreationDtoValidator : VisitaPersonaForManipulationDtoValidator<VisitaPersonaForCreationDto>
    {
        public VisitaPersonaForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
