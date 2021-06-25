using AutoMapper;
using VisitPop.Application.Dtos.Office;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            //createmap<to this, from this>
            CreateMap<Office, OfficeDto>()
                .ReverseMap();
            CreateMap<OfficeForCreationDto, Office>();
            CreateMap<OfficeForUpdateDto, Office>()
                .ReverseMap();
        }
    }
}
