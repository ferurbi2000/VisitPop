using VisitPop.Application.Dtos.PersonType;

namespace VisitPop.Application.Validation.PersonType
{
    public class PersonTypeForCreationDtoValidator : PersonTypeForManipulationDtoValidator<PersonTypeForCreationDto>
    {
        public PersonTypeForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
