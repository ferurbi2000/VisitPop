using System.Threading.Tasks;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.VehicleType
{
    public interface IVehicleTypeRepository
    {
        Task<PagingResponse<VehicleTypeDto>> GetVehicleTypesAsync(VehicleTypeParametersDto vehicleTypeParameters);
        Task<VehicleTypeDto> GetVehicleType(int id);
        Task<VehicleTypeDto> AddVehicleType(VehicleTypeDto vehicleType);
        Task UpdateVehicleType(VehicleTypeDto vehicleType);
        Task DeleteVehicleType(VehicleTypeDto vehicleType);
    }
}
