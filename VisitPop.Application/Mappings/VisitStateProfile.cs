using AutoMapper;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VisitStateProfile : Profile
    {
        public VisitStateProfile()
        {
            //createmap<to this, from this>
            CreateMap<VisitState, VisitStateDto>()
                .ReverseMap();
            CreateMap<VisitStateForCreationDto, VisitState>();
            CreateMap<VisitStateForUpdateDto, VisitState>()
                .ReverseMap();
        }
    }
}
