using Newtonsoft.Json;
using System.Collections.Generic;
using VisitPop.Application.Dtos.Person;

namespace VisitPop.Application.Responses
{
    public partial class PersonResponseDto : BasePageResponse
    {
        [JsonProperty("data")]
        public PersonDto Person { get; set; }
    }

    public partial class PageListPerson : BasePageResponse
    {
        [JsonProperty("data")]
        public List<PersonDto> Persons { get; set; }
    }
}
