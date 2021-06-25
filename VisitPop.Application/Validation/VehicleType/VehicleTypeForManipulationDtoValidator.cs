using FluentValidation;
using VisitPop.Application.Dtos.VehicleType;

namespace VisitPop.Application.Validation.VehicleType
{
    public class VehicleTypeForManipulationDtoValidator<T> : AbstractValidator<T> where T : VehicleTypeForManipulationDto
    {
        public VehicleTypeForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
