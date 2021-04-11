using AutoMapper;
using VisitPop.Application.Dtos.VisitaPersona;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VisitaPersonaProfile : Profile
    {
        public VisitaPersonaProfile()
        {
            //createmap<to this, from this>
            CreateMap<VisitaPersona, VisitaPersonaDto>()
                .ReverseMap();
            CreateMap<VisitaPersonaForCreationDto, VisitaPersona>();
            CreateMap<VisitaPersonaForUpdateDto, VisitaPersona>()
                .ReverseMap();
        }
    }
}
