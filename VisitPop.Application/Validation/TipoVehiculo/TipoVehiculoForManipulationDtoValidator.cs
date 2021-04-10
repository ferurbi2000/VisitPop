using FluentValidation;
using VisitPop.Application.Dtos.TipoVehiculo;

namespace VisitPop.Application.Validation.TipoVehiculo
{
    public class TipoVehiculoForManipulationDtoValidator<T> : AbstractValidator<T> where T : TipoVehiculoForManipulationDto
    {
        public TipoVehiculoForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
