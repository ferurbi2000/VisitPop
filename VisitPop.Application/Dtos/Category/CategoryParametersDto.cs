using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Category
{
    public class CategoryParametersDto: BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
