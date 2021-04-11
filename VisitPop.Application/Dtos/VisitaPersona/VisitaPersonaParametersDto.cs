using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.VisitaPersona
{
    public class VisitaPersonaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
