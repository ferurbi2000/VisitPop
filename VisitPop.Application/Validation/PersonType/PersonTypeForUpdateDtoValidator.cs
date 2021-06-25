using VisitPop.Application.Dtos.PersonType;

namespace VisitPop.Application.Validation.PersonType
{
    public class PersonTypeForUpdateDtoValidator : PersonTypeForManipulationDtoValidator<PersonTypeForUpdatedDto>
    {
        public PersonTypeForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
