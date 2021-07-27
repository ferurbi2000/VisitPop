using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.VisitPerson
{
    public class VisitPersonParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
