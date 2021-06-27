using VisitPop.Application.Dtos.Person;

namespace VisitPop.Application.Validation.Person
{
    public class PersonForCreationDtoValidator : PersonForManipulatiosDtoValidator<PersonForCreationDto>
    {
        public PersonForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
