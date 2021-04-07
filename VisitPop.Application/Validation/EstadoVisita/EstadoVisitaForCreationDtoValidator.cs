using VisitPop.Application.Dtos.EstadoVisita;

namespace VisitPop.Application.Validation.EstadoVisita
{
    public class EstadoVisitaForCreationDtoValidator : EstadoVisitaForManipulationDtoValidator<EstadoVisitaForCreationDto>
    {
        public EstadoVisitaForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
