using AutoMapper;
using VisitPop.Application.Dtos.Person;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            //createmap<to this, from this>
            CreateMap<Person, PersonDto>()
                .ReverseMap();
            CreateMap<PersonForCreationDto, Person>();
            CreateMap<PersonForUpdateDto, Person>()
                .ReverseMap();
        }
    }
}
