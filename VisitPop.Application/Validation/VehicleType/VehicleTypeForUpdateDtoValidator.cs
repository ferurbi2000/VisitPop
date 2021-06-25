using VisitPop.Application.Dtos.VehicleType;

namespace VisitPop.Application.Validation.VehicleType
{
    public class VehicleTypeForUpdateDtoValidator : VehicleTypeForManipulationDtoValidator<VehicleTypeForUpdateDto>
    {
        // add fluent validation rules that should be shared between creation and update operations here
        //https://fluentvalidation.net/
    }
}