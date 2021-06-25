using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.VisitState;

namespace VisitPop.Application.Responses
{
    public partial class VisitStateResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public VisitStateDto VisitState { get; set; }
    }

    public partial class PageListVisitState : BasePageResponse
    {
        [JsonProperty("data")]
        public List<VisitStateDto> VisitStates { get; set; }
    }
}
