using VisitPop.Application.Dtos.VisitType;

namespace VisitPop.Application.Validation.VisitType
{
    public class VisitTypeForUpdateDtoValidator : VisitTypeForManipulationDtoValidator<VisitTypeForUpdateDto>
    {
        public VisitTypeForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
