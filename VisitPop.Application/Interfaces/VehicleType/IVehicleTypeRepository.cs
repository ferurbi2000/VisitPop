namespace VisitPop.Application.Interfaces.VehicleType
{
    using Application.Wrappers;
    using Domain.Entities;
    using global::VisitPop.Application.Dtos.VehicleType;
    using System.Threading.Tasks;

    public interface IVehicleTypeRepository
    {
        Task<PagedList<VehicleType>> GetVehicleTypesAsync(VehicleTypeParametersDto vehicleTypeParameters);
        Task<VehicleType> GetVehicleTypeAsync(int id);
        VehicleType GetVehicleType(int id);
        Task AddVehicleType(VehicleType vehicleType);
        void DeleteVehicleType(VehicleType vehicleType);
        void UpdateVehicleType(VehicleType vehicleType);
        bool Save();
        Task<bool> SaveAsync();
    }
}
