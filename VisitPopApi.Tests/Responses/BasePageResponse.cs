using Newtonsoft.Json;

namespace VisitPopApi.Tests.Responses
{
    public class BasePageResponse
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }
    }
}
