using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.DepartamentoEmpleado;

namespace VisitPopApi.Tests.Responses
{
    public partial class DepartamentoEmpleadoResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public DepartamentoEmpleadoDto DepartamentoEmpleado { get; set; }
    }

    public partial class PageListDepartamentoEmpleado : BasePageResponse
    {

        [JsonProperty("data")]
        public List<DepartamentoEmpleadoDto> DepartamentoEmpleados { get; set; }
    }
}
