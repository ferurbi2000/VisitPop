using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.EmployeeDepartment
{
    public class EmployeeDepartmentParametersDto: BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
