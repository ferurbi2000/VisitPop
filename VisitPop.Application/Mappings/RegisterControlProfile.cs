using AutoMapper;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class RegisterControlProfile : Profile
    {
        public RegisterControlProfile()
        {
            //createmap<to this, from this>
            CreateMap<RegisterControl, RegisterControlDto>()
                .ReverseMap();
            CreateMap<RegisterControlForCreationDto, RegisterControl>();
            CreateMap<RegisterControlForUpdateDto, RegisterControl>()
                .ReverseMap();
        }
    }
}
