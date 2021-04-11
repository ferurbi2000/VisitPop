using AutoMapper;
using VisitPop.Application.Dtos.TipoVisita;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class TipoVisitaProfile : Profile
    {
        public TipoVisitaProfile()
        {
            //createmap<to this, from this>
            CreateMap<TipoVisita, TipoVisitaDto>()
                .ReverseMap();
            CreateMap<TipoVisitaForCreationDto, TipoVisita>();
            CreateMap<TipoVisitaForUpdateDto, TipoVisita>()
                .ReverseMap();
        }

    }
}
