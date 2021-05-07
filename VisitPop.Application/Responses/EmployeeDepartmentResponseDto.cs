using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.EmployeeDepartment;

namespace VisitPop.Application.Responses
{
    public partial class EmployeeDepartmentResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public EmployeeDepartmentDto EmployeeDepartment { get; set; }
    }

    public partial class PageListEmployeeDepartment : BasePageResponse
    {

        [JsonProperty("data")]
        public List<EmployeeDepartmentDto> EmployeeDepartments { get; set; }
    }
}
