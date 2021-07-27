using VisitPop.Application.Dtos.VisitPerson;

namespace VisitPop.Application.Validation.VisitPerson
{
    public class VisitPersonForCreationDtoValidator : VisitPersonForManipulationDtoValidator<VisitPersonForCreationDto>
    {
        public VisitPersonForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
