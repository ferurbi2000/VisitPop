using AutoMapper;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class PersonTypeProfile : Profile
    {
        public PersonTypeProfile()
        {
            //createmap<to this, from this>
            CreateMap<PersonType, PersonTypeDto>().
                ReverseMap();
            CreateMap<PersonTypeForCreationDto, PersonType>();
            CreateMap<PersonTypeForUpdatedDto, PersonType>()
                .ReverseMap();
        }
    }
}
