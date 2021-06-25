using FluentValidation;
using VisitPop.Application.Dtos.Office;

namespace VisitPop.Application.Validation.Office
{
    public class OfficeForManipulationDtoValidator<T> : AbstractValidator<T> where T : OfficeForManipulationDto
    {
        public OfficeForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
