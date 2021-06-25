using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Office;

namespace VisitPop.Application.Responses
{
    public class OfficeResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public OfficeDto Office { get; set; }
    }

    public partial class PageListOffice : BasePageResponse
    {
        [JsonProperty("data")]
        public List<OfficeDto> Offices { get; set; }
    }
}
