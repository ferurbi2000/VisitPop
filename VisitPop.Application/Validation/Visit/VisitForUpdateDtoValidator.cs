using VisitPop.Application.Dtos.Visit;

namespace VisitPop.Application.Validation.Visit
{
    public class VisitForUpdateDtoValidator : VisitForManipulationDtoValidator<VisitForUpdateDto>
    {
        public VisitForUpdateDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
