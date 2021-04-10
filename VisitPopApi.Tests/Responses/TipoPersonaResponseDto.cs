using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoPersona;

namespace VisitPopApi.Tests.Responses
{
    public partial class TipoPersonaResponseDto: BasePageResponse
    {
        
        [JsonProperty("data")]
        public TipoPersonaDto TipoPersona { get; set; }

    }
   
    public partial class PageListTipoPersona: BasePageResponse
    {

        [JsonProperty("data")]
        public List<TipoPersonaDto> TipoPersonas { get; set; }
    }
}
