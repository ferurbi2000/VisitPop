using FluentValidation;
using VisitPop.Application.Dtos.PuntoControl;

namespace VisitPop.Application.Validation.PuntoControl
{
    public class PuntoControlForManipulationDtoValidator<T> : AbstractValidator<T> where T : PuntoControlForManipulationDto
    {
        public PuntoControlForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
