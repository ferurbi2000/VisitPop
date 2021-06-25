using VisitPop.Application.Dtos.Shared;

namespace VisitPop.Application.Dtos.VehicleType
{
    public class VehicleTypeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
