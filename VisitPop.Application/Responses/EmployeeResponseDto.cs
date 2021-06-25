using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Employee;

namespace VisitPop.Application.Responses
{
    public partial class EmployeeResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public EmployeeDto Employee { get; set; }
    }

    public partial class PageListEmployee : BasePageResponse
    {
        [JsonProperty("data")]
        public List<EmployeeDto> Employees { get; set; }
    }
}
