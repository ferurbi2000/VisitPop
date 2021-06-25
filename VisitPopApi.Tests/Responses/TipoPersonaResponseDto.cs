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
        public PersonTypeDto TipoPersona { get; set; }

    }
   
    public partial class PageListTipoPersona: BasePageResponse
    {

        [JsonProperty("data")]
        public List<PersonTypeDto> TipoPersonas { get; set; }
    }
}
