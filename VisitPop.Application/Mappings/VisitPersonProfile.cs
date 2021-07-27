using AutoMapper;
using VisitPop.Application.Dtos.VisitPerson;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class VisitPersonProfile : Profile
    {
        public VisitPersonProfile()
        {
            //createmap<to this, from this>
            CreateMap<VisitPerson, VisitPersonDto>()
                .ReverseMap();
            CreateMap<VisitPersonForCreationDto, VisitPerson>();
            CreateMap<VisitPersonForUpdateDto, VisitPerson>()
                .ReverseMap();
        }
    }
}
