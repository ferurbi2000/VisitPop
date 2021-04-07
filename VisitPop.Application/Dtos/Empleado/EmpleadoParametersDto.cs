using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.Empleado
{
    public class EmpleadoParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
