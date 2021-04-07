using VisitPop.Application.Dtos.Category;
using FluentValidation;

namespace VisitPop.Application.Validation.Category
{
    public class CategoryForUpdateDtoValidator: CategoryForManipulationDtoValidator<CategoryForUpdateDto>
    {
        public CategoryForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
