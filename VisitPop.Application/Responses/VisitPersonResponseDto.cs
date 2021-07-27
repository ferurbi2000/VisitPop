using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.VisitPerson;

namespace VisitPop.Application.Responses
{
    public partial class VisitPersonResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public VisitPersonDto VisitPerson { get; set; }
    }

    public partial class PageListVisitPersons : BasePageResponse
    {
        [JsonProperty("data")]
        public List<VisitPersonDto> VisitPersons { get; set; }
    }
}
