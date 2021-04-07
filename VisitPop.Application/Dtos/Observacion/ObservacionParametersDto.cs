using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Observacion
{
    public class ObservacionParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
