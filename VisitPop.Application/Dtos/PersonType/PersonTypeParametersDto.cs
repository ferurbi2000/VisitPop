using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.PersonType
{
    public class PersonTypeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
