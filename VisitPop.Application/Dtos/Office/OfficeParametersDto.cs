using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Office
{
    public class OfficeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
