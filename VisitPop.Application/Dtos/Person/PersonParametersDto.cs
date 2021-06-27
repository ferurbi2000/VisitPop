using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Person
{
    public class PersonParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
