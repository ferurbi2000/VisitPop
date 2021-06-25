using FluentValidation;
using VisitPop.Application.Dtos.VisitState;

namespace VisitPop.Application.Validation.VisitState
{
    public class VisitStateForManipulationDtoValidator<T> : AbstractValidator<T> where T : VisitStateForManipulationDto
    {
        public VisitStateForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
