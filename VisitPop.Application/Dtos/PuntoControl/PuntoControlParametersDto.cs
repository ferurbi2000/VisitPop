using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.PuntoControl
{
    public class PuntoControlParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
