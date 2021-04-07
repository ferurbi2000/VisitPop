using AutoMapper;
using VisitPop.Application.Dtos.Oficina;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class OficinaProfile : Profile
    {
        public OficinaProfile()
        {
            //createmap<to this, from this>
            CreateMap<Oficina, OficinaDto>()
                .ReverseMap();
            CreateMap<OficinaForCreationDto, Oficina>();
            CreateMap<OficinaForUpdateDto, Oficina>()
                .ReverseMap();
        }
    }
}
