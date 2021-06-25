using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.VehicleType;

namespace VisitPop.Application.Responses
{
    public partial class VehicleTypeResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public VehicleTypeDto VehicleType { get; set; }
    }

    public partial class PageListVehicleType : BasePageResponse
    {
        [JsonProperty("data")]
        public List<VehicleTypeDto> VehicleTypes { get; set; }
    }
}
