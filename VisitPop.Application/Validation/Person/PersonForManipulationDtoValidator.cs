using FluentValidation;
using VisitPop.Application.Dtos.Person;

namespace VisitPop.Application.Validation.Person
{
    public class PersonForManipulationDtoValidator<T>: AbstractValidator<T> where T: PersonForManipulationDto
    {
        public PersonForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
