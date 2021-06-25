using VisitPop.Application.Dtos.Company;

namespace VisitPop.Application.Validation.Company
{
    public class CompanyForCreationDtoValidator : CompanyForManipulationDtoValidator<CompanyForCreationDto>
    {
        public CompanyForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
