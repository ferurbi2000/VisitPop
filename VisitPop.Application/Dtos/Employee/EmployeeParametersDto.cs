using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Employee
{
    public class EmployeeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
