using FluentValidation;
using VisitPop.Application.Dtos.VisitaPersona;

namespace VisitPop.Application.Validation.VisitaPersona
{
    public class VisitaPersonaForManipulationDtoValidator<T> : AbstractValidator<T> where T : VisitaPersonaForManipulationDto
    {
        public VisitaPersonaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
