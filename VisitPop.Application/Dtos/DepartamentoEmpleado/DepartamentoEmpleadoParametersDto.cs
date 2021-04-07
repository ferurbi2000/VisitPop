using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.DepartamentoEmpleado
{
    public class DepartamentoEmpleadoParametersDto: BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
