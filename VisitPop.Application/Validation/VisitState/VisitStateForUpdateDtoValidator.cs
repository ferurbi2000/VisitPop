using VisitPop.Application.Dtos.VisitState;

namespace VisitPop.Application.Validation.VisitState
{
    public class VisitStateForUpdateDtoValidator : VisitStateForManipulationDtoValidator<VisitStateForUpdateDto>
    {
        public VisitStateForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
