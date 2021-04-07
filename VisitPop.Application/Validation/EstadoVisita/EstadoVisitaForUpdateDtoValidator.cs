using VisitPop.Application.Dtos.EstadoVisita;

namespace VisitPop.Application.Validation.EstadoVisita
{
    public class EstadoVisitaForUpdateDtoValidator : EstadoVisitaForManipulationDtoValidator<EstadoVisitaForUpdateDto>
    {
        public EstadoVisitaForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
