namespace VisitPop.Application.Interfaces.EstadoVisita
{
    using Application.Wrappers;
    using Domain.Entities;
    using Application.Dtos.EstadoVisita;
    using System.Threading.Tasks;

    public interface IEstadoVisitaRepository
    {
        Task<PagedList<EstadoVisita>> GetEstadoVisitasAsync(EstadoVisitaParametersDto estadoVisitaParametersDto);
        Task<EstadoVisita> GetEstadoVisitaAsync(int estadoVisitaId);
        EstadoVisita GetEstadoVisita(int estadoVisitaId);
        Task AddEstadoVisita(EstadoVisita estadoVisita);
        void DeleteEstadoVisita(EstadoVisita estadoVisita);
        void UpdateEstadoVisita(EstadoVisita estadoVisita);
        bool Save();
        Task<bool> SaveAsync();
    }
}
