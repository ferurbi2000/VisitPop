using AutoMapper;
using VisitPop.Application.Dtos.PuntoControl;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class PuntoControlProfile : Profile
    {
        public PuntoControlProfile()
        {
            //createmap<to this, from this>
            CreateMap<PuntoControl, PuntoControlDto>()
                .ReverseMap();
            CreateMap<PuntoControlForCreationDto, PuntoControl>();
            CreateMap<PuntoControlForUpdateDto, PuntoControl>()
                .ReverseMap();
        }
    }
}
