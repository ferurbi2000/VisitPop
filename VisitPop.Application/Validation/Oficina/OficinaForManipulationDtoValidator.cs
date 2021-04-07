using FluentValidation;
using VisitPop.Application.Dtos.Oficina;

namespace VisitPop.Application.Validation.Oficina
{
    public class OficinaForManipulationDtoValidator<T> : AbstractValidator<T> where T : OficinaForManipulationDto
    {
        public OficinaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
