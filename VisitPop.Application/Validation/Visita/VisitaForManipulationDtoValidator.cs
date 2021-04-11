using FluentValidation;
using VisitPop.Application.Dtos.Visita;

namespace VisitPop.Application.Validation.Visita
{
    public class VisitaForManipulationDtoValidator<T> : AbstractValidator<T> where T : VisitaForManipulationDto
    {
        public VisitaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
