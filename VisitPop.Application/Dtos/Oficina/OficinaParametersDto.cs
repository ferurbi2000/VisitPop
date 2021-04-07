using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Oficina
{
    public class OficinaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
