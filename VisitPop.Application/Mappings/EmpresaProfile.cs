using AutoMapper;
using VisitPop.Application.Dtos.Empresa;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            //createmap<to this, from this>
            CreateMap<Empresa, EmpresaDto>()
                .ReverseMap();
            CreateMap<EmpresaForCreationDto, Empresa>();
            CreateMap<EmpresaForUpdateDto, Empresa>()
                .ReverseMap();
        }

    }
}
