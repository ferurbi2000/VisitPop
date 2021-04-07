using FluentValidation;
using VisitPop.Application.Dtos.EstadoVisita;

namespace VisitPop.Application.Validation.EstadoVisita
{
    public class EstadoVisitaForManipulationDtoValidator<T> : AbstractValidator<T> where T : EstadoVisitaForManipulationDto
    {
        public EstadoVisitaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
