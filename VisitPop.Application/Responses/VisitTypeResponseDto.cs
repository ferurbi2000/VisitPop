using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.VisitType;

namespace VisitPop.Application.Responses
{
    public partial class VisitTypeResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public VisitTypeDto VisitType { get; set; }
    }

    public partial class PageListVisitType : BasePageResponse
    {
        [JsonProperty("data")]
        public List<VisitTypeDto> VisitTypes { get; set; }
    }
}
