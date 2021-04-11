namespace VisitPop.Application.Interfaces.Visita
{
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;
    using VisitPop.Application.Dtos.Visita;

    public interface IVisitaRepository
    {
        Task<PagedList<Visita>> GetVisitasAsync(VisitaParametersDto visitaParameters);
        Task<Visita> GetVisitaAsync(int id);
        Visita GetVisita(int id);
        Task AddVisita(Visita visita);
        void DeleteVisita(Visita visita);
        void UpdateVisita(Visita visita);
        bool Save();
        Task<bool> SaveAsync();
    }
}
