using VisitPop.Application.Dtos.TipoVehiculo;

namespace VisitPop.Application.Validation.TipoVehiculo
{
    public class TipoVehiculoForUpdateDtoValidator : TipoVehiculoForManipulationDtoValidator<TipoVehiculoForUpdateDto>
    {
        // add fluent validation rules that should be shared between creation and update operations here
        //https://fluentvalidation.net/
    }
}
