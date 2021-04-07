using VisitPop.Application.Dtos.Category;
using FluentValidation;

namespace VisitPop.Application.Validation.Category
{
    public class CategoryForCreationDtoValidator: CategoryForManipulationDtoValidator<CategoryForCreationDto>
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}
