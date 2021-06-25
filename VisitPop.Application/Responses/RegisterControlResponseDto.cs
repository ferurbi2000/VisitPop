using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.RegisterControl;

namespace VisitPop.Application.Responses
{
    public partial class RegisterControlResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public RegisterControlDto RegisterControl { get; set; }
    }

    public partial class PageListRegisterControl : BasePageResponse
    {
        [JsonProperty("data")]
        public List<RegisterControlDto> RegisterControls { get; set; }
    }
}
