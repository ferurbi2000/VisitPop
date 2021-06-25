using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.RegisterControl
{
    public class RegisterControlParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
