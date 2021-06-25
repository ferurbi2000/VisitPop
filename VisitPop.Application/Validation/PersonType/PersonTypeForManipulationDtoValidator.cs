using FluentValidation;
using VisitPop.Application.Dtos.PersonType;

namespace VisitPop.Application.Validation.PersonType
{
    public class PersonTypeForManipulationDtoValidator<T> : AbstractValidator<T> where T : PersonTypeForManipulationDto
    {
        public PersonTypeForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
