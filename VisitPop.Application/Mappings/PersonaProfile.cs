using AutoMapper;
using VisitPop.Application.Dtos.Persona;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class PersonaProfile : Profile
    {
        public PersonaProfile()
        {
            //createmap<to this, from this>
            CreateMap<Persona, PersonaDto>()
                .ReverseMap();
            CreateMap<PersonaForCreationDto, Persona>();
            CreateMap<PersonaForUpdateDto, Persona>()
                .ReverseMap();
        }
    }
}
