using FluentValidation;
using VisitPop.Application.Dtos.Observacion;

namespace VisitPop.Application.Validation.Observacion
{
    public class ObservacionForManipulationDtoValidator<T> : AbstractValidator<T> where T : ObservacionForManipulationDto
    {
        public ObservacionForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
