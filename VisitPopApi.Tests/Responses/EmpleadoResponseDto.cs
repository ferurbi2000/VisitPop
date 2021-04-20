using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Empleado;

namespace VisitPopApi.Tests.Responses
{
    public partial class EmpleadoResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public EmpleadoDto Empleado { get; set; }
    }

    public partial class PageListEmpleado : BasePageResponse
    {

        [JsonProperty("data")]
        public List<EmpleadoDto> Empleados { get; set; }
    }
}
