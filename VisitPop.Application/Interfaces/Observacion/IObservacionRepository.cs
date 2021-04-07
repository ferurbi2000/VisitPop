namespace VisitPop.Application.Interfaces.Observacion
{
    using Application.Dtos.Observacion;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;
    public interface IObservacionRepository
    {
        Task<PagedList<Observacion>> GetObservacionesAsync(ObservacionParametersDto observacionParameters);
        Task<Observacion> GetObservacionAsync(int id);
        Observacion GetObservacion(int id);
        Task AddObservacion(Observacion observacion);
        void DeleteObservacion(Observacion observacion);
        void UpdateObservacion(Observacion observacion);
        bool Save();
        Task<bool> SaveAsync();
    }
}
