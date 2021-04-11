using FluentValidation;
using VisitPop.Application.Dtos.Persona;

namespace VisitPop.Application.Validation.Persona
{
    public class PersonaForManipulatiosDtoValidator<T> : AbstractValidator<T> where T : PersonaForManipulationDto
    {
        public PersonaForManipulatiosDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
