using FluentValidation;
using VisitPop.Application.Dtos.Category;

namespace VisitPop.Application.Validation.Category
{
    public class CategoryForManipulationDtoValidator<T>: AbstractValidator<T> where T: CategoryForManipulationDto
    {
        public CategoryForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}