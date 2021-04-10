namespace VisitPop.Application.Interfaces.TipoVehiculo
{
    using Application.Wrappers;
    using Domain.Entities;
    using global::VisitPop.Application.Dtos.TipoVehiculo;
    using System.Threading.Tasks;

    public interface ITipoVehiculoRepository
    {
        Task<PagedList<TipoVehiculo>> GetTipoVehiculosAsync(TipoVehiculoParametersDto tipoVehiculoParameters);
        Task<TipoVehiculo> GetTipoVehiculoAsync(int id);
        TipoVehiculo GetTipoVehiculo(int id);
        Task AddTipoVehiculo(TipoVehiculo tipoVehiculo);
        void DeleteTipoVehiculo(TipoVehiculo tipoVehiculo);
        void UpdateTipoVehiculo(TipoVehiculo tipoVehiculo);
        bool Save();
        Task<bool> SaveAsync();
    }
}
