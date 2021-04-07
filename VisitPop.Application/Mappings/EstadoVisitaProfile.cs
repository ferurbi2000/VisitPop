using AutoMapper;
using VisitPop.Application.Dtos.EstadoVisita;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class EstadoVisitaProfile : Profile
    {
        public EstadoVisitaProfile()
        {
            //createmap<to this, from this>
            CreateMap<EstadoVisita, EstadoVisitaDto>()
                .ReverseMap();
            CreateMap<EstadoVisitaForCreationDto, EstadoVisita>();
            CreateMap<EstadoVisitaForUpdateDto, EstadoVisita>()
                .ReverseMap();
        }
    }
}
