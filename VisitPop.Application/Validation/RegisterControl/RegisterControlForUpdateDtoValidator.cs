using VisitPop.Application.Dtos.RegisterControl;

namespace VisitPop.Application.Validation.RegisterControl
{
    public class RegisterControlForUpdateDtoValidator : RegisterControlForManipulationDtoValidator<RegisterControlForUpdateDto>
    {
        // add fluent validation rules that should be shared between creation and update operations here
        //https://fluentvalidation.net/
    }
}
