using VisitPop.Application.Dtos.VisitState;

namespace VisitPop.Application.Validation.VisitState
{
    public class VisitStateForCreationDtoValidator : VisitStateForManipulationDtoValidator<VisitStateForCreationDto>
    {
        public VisitStateForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
