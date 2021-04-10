using AutoMapper;
using VisitPop.Application.Dtos.TipoVehiculo;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class TipoVehiculoProfile : Profile
    {
        public TipoVehiculoProfile()
        {
            //createmap<to this, from this>
            CreateMap<TipoVehiculo, TipoVehiculoDto>()
                .ReverseMap();
            CreateMap<TipoVehiculoForCreationDto, TipoVehiculo>();
            CreateMap<TipoVehiculoForUpdateDto, TipoVehiculo>()
                .ReverseMap();
        }
    }
}
