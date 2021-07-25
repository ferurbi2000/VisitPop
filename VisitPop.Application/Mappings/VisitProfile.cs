using AutoMapper;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VisitProfile : Profile
    {
        public VisitProfile()
        {
            //createmap<to this, from this>
            CreateMap<Visit, VisitDto>()
                .ReverseMap();
            CreateMap<VisitForCreationDto, Visit>();
            CreateMap<VisitForUpdateDto, Visit>()
                .ReverseMap();
        }
    }
}
