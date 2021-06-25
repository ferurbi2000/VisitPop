using AutoMapper;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VehicleTypeProfile : Profile
    {
        public VehicleTypeProfile()
        {
            //createmap<to this, from this>
            CreateMap<VehicleType, VehicleTypeDto>()
                .ReverseMap();
            CreateMap<VehicleTypeForCreationDto, VehicleType>();
            CreateMap<VehicleTypeForUpdateDto, VehicleType>()
                .ReverseMap();
        }
    }
}
