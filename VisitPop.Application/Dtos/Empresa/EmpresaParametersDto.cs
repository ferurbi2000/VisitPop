using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Empresa
{
    public class EmpresaParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
