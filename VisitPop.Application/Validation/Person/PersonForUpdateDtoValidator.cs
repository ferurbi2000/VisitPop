using VisitPop.Application.Dtos.Person;

namespace VisitPop.Application.Validation.Person
{
    public class PersonForUpdateDtoValidator : PersonForManipulatiosDtoValidator<PersonForUpdateDto>
    {
        public PersonForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
