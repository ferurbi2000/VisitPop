using VisitPop.Application.Dtos.Person;

namespace VisitPop.Application.Validation.Person
{
    public class PersonForUpdateDtoValidator: PersonForManipulationDtoValidator<PersonForUpdateDto>
    {
        public PersonForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
