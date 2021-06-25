using VisitPop.Application.Dtos.Company;

namespace VisitPop.Application.Validation.Company
{
    public class CompanyForUpdateDtoValidator : CompanyForManipulationDtoValidator<CompanyForUpdateDto>
    {
        public CompanyForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
