using FluentValidation;
using VisitPop.Application.Dtos.Visit;

namespace VisitPop.Application.Validation.Visit
{
    public class VisitForManipulationDtoValidator<T> : AbstractValidator<T> where T : VisitForManipulationDto
    {
        public VisitForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
