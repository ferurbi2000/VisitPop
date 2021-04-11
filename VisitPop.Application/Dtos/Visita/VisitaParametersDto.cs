using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Visita
{
    public class VisitaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
