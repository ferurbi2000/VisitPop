using VisitPop.Application.Dtos.Oficina;

namespace VisitPop.Application.Validation.Oficina
{
    public class OficinaForCreationDtoValidator : OficinaForManipulationDtoValidator<OficinaForCreationDto>
    {
        public OficinaForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
