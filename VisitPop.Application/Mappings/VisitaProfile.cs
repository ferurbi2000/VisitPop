using AutoMapper;
using VisitPop.Application.Dtos.Visita;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VisitaProfile : Profile
    {
        public VisitaProfile()
        {
            //createmap<to this, from this>
            CreateMap<Visita, VisitaDto>()
                .ReverseMap();
            CreateMap<VisitaForCreationDto, Visita>();
            CreateMap<VisitaForUpdateDto, Visita>()
                .ReverseMap();
        }
    }
}
