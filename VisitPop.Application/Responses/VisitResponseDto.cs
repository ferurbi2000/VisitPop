using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Visit;

namespace VisitPop.Application.Responses
{
    public partial class VisitResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public VisitDto Visit { get; set; }
    }

    public partial class PageListVisit : BasePageResponse
    {
        [JsonProperty("data")]
        public List<VisitDto> Visits { get; set; }
    }
}
