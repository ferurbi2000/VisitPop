using FluentValidation;
using VisitPop.Application.Dtos.RegisterControl;

namespace VisitPop.Application.Validation.RegisterControl
{
    public class RegisterControlForManipulationDtoValidator<T> : AbstractValidator<T> where T : RegisterControlForManipulationDto
    {
        public RegisterControlForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
