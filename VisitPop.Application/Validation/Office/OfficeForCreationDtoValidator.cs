using VisitPop.Application.Dtos.Office;

namespace VisitPop.Application.Validation.Office
{
    public class OfficeForCreationDtoValidator : OfficeForManipulationDtoValidator<OfficeForCreationDto>
    {
        public OfficeForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
