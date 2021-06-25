using VisitPop.Application.Dtos.VehicleType;

namespace VisitPop.Application.Validation.VehicleType
{
    public class VehicleTypeForCreationDtoValidator : VehicleTypeForManipulationDtoValidator<VehicleTypeForCreationDto>
    {
        public VehicleTypeForCreationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
