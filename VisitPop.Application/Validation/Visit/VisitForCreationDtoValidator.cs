using VisitPop.Application.Dtos.Visit;

namespace VisitPop.Application.Validation.Visit
{
    public class VisitForCreationDtoValidator : VisitForManipulationDtoValidator<VisitForCreationDto>
    {
        public VisitForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
