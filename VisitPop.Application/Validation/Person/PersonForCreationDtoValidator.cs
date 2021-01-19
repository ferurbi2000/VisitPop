using VisitPop.Application.Dtos.Person;

namespace VisitPop.Application.Validation.Person
{
    public class PersonForCreationDtoValidator:PersonForManipulationDtoValidator<PersonForCreationDto>
    {
        public PersonForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
