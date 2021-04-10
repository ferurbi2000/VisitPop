using AutoMapper;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class TipoPersonaProfile : Profile
    {
        public TipoPersonaProfile()
        {
            //createmap<to this, from this>
            CreateMap<TipoPersona, TipoPersonaDto>().
                ReverseMap();
            CreateMap<TipoPersonaForCreationDto, TipoPersona>();
            CreateMap<TipoPersonaForUpdatedDto, TipoPersona>()
                .ReverseMap();
        }
    }
}
