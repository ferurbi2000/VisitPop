using VisitPop.Application.Dtos.Visita;

namespace VisitPop.Application.Validation.Visita
{
    public class VisitaForCreationDtoValidator : VisitaForManipulationDtoValidator<VisitaForCreationDto>
    {
        public VisitaForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
