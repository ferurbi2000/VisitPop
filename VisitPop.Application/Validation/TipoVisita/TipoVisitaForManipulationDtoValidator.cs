using FluentValidation;
using VisitPop.Application.Dtos.TipoVisita;

namespace VisitPop.Application.Validation.TipoVisita
{
    public class TipoVisitaForManipulationDtoValidator<T> : AbstractValidator<T> where T : TipoVisitaForManipulationDto
    {
        public TipoVisitaForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
