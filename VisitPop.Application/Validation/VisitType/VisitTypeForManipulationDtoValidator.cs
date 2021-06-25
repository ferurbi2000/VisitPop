using FluentValidation;
using VisitPop.Application.Dtos.VisitType;

namespace VisitPop.Application.Validation.VisitType
{
    public class VisitTypeForManipulationDtoValidator<T> : AbstractValidator<T> where T : VisitTypeForManipulationDto
    {
        public VisitTypeForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
