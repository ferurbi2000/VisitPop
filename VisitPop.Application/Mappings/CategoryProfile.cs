using AutoMapper;
using VisitPop.Application.Dtos.Category;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap();
            CreateMap<CategoryForCreationDto, Category>();
            CreateMap<CategoryForUpdateDto, Category>()
                .ReverseMap();
        }
    }
}
