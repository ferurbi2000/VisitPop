using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.PersonType;

namespace VisitPop.Application.Responses
{
    public partial class PersonTypeResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public PersonTypeDto PersonType { get; set; }
    }

    public partial class PageListPersonType : BasePageResponse
    {
        [JsonProperty("data")]
        public List<PersonTypeDto> PersonTypes { get; set; }
    }
}
