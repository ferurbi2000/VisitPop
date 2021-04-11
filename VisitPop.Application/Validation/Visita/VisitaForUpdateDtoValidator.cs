using VisitPop.Application.Dtos.Visita;

namespace VisitPop.Application.Validation.Visita
{
    public class VisitaForUpdateDtoValidator : VisitaForManipulationDtoValidator<VisitaForUpdateDto>
    {
        public VisitaForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
