using FluentValidation;
using VisitPop.Application.Dtos.VisitPerson;

namespace VisitPop.Application.Validation.VisitPerson
{
    public class VisitPersonForManipulationDtoValidator<T> : AbstractValidator<T> where T : VisitPersonForManipulationDto
    {
        public VisitPersonForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
