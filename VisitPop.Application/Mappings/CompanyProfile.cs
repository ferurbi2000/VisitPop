using AutoMapper;
using VisitPop.Application.Dtos.Company;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            //createmap<to this, from this>
            CreateMap<Company, CompanyDto>()
                .ReverseMap();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<CompanyForUpdateDto, Company>()
                .ReverseMap();
        }

    }
}
