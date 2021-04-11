using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.TipoVisita
{
    public class TipoVisitaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
