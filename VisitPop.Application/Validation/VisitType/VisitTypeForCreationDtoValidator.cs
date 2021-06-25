using VisitPop.Application.Dtos.VisitType;

namespace VisitPop.Application.Validation.VisitType
{
    public class VisitTypeForCreationDtoValidator : VisitTypeForManipulationDtoValidator<VisitTypeForCreationDto>
    {
        public VisitTypeForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
