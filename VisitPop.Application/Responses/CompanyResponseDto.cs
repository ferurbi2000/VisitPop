using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Company;

namespace VisitPop.Application.Responses
{
    public partial class CompanyResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public CompanyDto Company { get; set; }
    }

    public partial class PageListCompany : BasePageResponse
    {
        [JsonProperty("data")]
        public List<CompanyDto> Companies { get; set; }
    }
}
