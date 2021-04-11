using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Persona
{
    public class PersonaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
