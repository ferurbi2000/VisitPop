using AutoMapper;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Domain.Entities;

namespace VisitPop.Application.Mappings
{
    public class EmployeeDepartmentProfile : Profile
    {
        public EmployeeDepartmentProfile()
        {
            //createmap<to this, from this>
            CreateMap<EmployeeDepartment, EmployeeDepartmentDto>()
                .ReverseMap();
            CreateMap<EmployeeDepartmentForCreationDto, EmployeeDepartment>();
            CreateMap<EmployeeDepartmentForUpdateDto, EmployeeDepartment>()
                .ReverseMap();
        }
    }
}
