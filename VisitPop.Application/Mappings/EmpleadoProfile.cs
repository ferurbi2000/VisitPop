using AutoMapper;
using VisitPop.Application.Dtos.Empleado;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class EmpleadoProfile : Profile
    {
        public EmpleadoProfile()
        {
            //createmap<to this, from this>
            CreateMap<Empleado, EmpleadoDto>()
                .ReverseMap();
            CreateMap<EmpleadoForCreationDto, Empleado>();
            CreateMap<EmpleadoForUpdateDto, Empleado>()
                .ReverseMap();
        }
    }
}
