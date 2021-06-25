using AutoMapper;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VisitTypeProfile : Profile
    {
        public VisitTypeProfile()
        {
            //createmap<to this, from this>
            CreateMap<VisitType, VisitTypeDto>()
                .ReverseMap();
            CreateMap<VisitTypeForCreationDto, VisitType>();
            CreateMap<VisitTypeForUpdateDto, VisitType>()
                .ReverseMap();
        }

    }
}
