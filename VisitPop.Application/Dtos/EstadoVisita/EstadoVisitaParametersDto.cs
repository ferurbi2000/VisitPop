using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.EstadoVisita
{
    public class EstadoVisitaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
