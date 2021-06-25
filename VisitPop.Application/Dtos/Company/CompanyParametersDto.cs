using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Company
{
    public class CompanyParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
