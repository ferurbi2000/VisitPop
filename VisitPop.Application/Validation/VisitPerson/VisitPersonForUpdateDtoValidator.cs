using VisitPop.Application.Dtos.VisitPerson;

namespace VisitPop.Application.Validation.VisitPerson
{
    public class VisitPersonForUpdateDtoValidator : VisitPersonForManipulationDtoValidator<VisitPersonForUpdateDto>
    {
        public VisitPersonForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
