using FluentValidation;
using VisitPop.Application.Dtos.TipoPersona;

namespace VisitPop.Application.Validation.TipoPersona
{
    public class TipoPersonaForManipulationDtoValidator<T> : AbstractValidator<T> where T : TipoPersonaForManipulationDto
    {
        public TipoPersonaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
