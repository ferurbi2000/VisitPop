using AutoMapper;
using VisitPop.Application.Dtos.Observacion;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class ObservacionProfile : Profile
    {
        public ObservacionProfile()
        {
            //createmap<to this, from this>
            CreateMap<Observacion, ObservacionDto>()
                .ReverseMap();
            CreateMap<ObservacionForCreationDto, Observacion>();
            CreateMap<ObservacionForUpdateDto, Observacion>()
                .ReverseMap();
        }
    }
}
