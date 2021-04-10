using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.TipoPersona
{
    public class TipoPersonaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
