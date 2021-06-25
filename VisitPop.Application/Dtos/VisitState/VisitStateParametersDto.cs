using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.VisitState
{
    public class VisitStateParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
