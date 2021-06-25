using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.VisitType
{
    public class VisitTypeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
