using AutoMapper;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            //createmap<to this, from this>
            CreateMap<Employee, EmployeeDto>()
                .ReverseMap();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>()
                .ReverseMap();
        }
    }
}
