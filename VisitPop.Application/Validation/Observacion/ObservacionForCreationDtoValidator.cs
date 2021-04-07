using VisitPop.Application.Dtos.Observacion;

namespace VisitPop.Application.Validation.Observacion
{
    public class ObservacionForCreationDtoValidator : ObservacionForManipulationDtoValidator<ObservacionForCreationDto>
    {
        public ObservacionForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
