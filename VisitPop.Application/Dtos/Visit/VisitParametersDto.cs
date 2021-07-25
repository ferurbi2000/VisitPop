using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Visit
{
    public class VisitParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
