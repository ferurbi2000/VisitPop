using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.TipoVehiculo
{
    public class TipoVehiculoParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
