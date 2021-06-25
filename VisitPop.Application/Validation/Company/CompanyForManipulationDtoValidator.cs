using FluentValidation;
using VisitPop.Application.Dtos.Company;

namespace VisitPop.Application.Validation.Company
{
    public class CompanyForManipulationDtoValidator<T> : AbstractValidator<T> where T : CompanyForManipulationDto
    {
        public CompanyForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
