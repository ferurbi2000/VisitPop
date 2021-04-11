using VisitPop.Application.Dtos.Persona;

namespace VisitPop.Application.Validation.Persona
{
    public class PersonaForUpdateDtoValidator : PersonaForManipulatiosDtoValidator<PersonaForUpdateDto>
    {
        public PersonaForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
