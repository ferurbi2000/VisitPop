using AutoMapper;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class DepartamentoEmpleadoProfile : Profile
    {
        public DepartamentoEmpleadoProfile()
        {
            //createmap<to this, from this>
            CreateMap<DepartamentoEmpleado, DepartamentoEmpleadoDto>()
                .ReverseMap();
            CreateMap<DepartamentoEmpleadoForCreationDto, DepartamentoEmpleado>();
            CreateMap<DepartamentoEmpleadoForUpdateDto, DepartamentoEmpleado>()
                .ReverseMap();
        }
    }
}
