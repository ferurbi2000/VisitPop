namespace VisitPop.Application.Interfaces.VisitaPersona
{
    using System.Threading.Tasks;
    using VisitPop.Application.Dtos.VisitaPersona;
    using VisitPop.Application.Wrappers;
    using VisitPop.Domain.Entities;

    public interface IVisitaPersonaRepository
    {
        Task<PagedList<VisitaPersona>> GetVisitaPersonasAsync(VisitaPersonaParametersDto visitaPersonaParameters);
        Task<VisitaPersona> GetVisitaPersonaAsync(int id);
        VisitaPersona GetVisitaPersona(int id);
        Task AddVisitaPersona(VisitaPersona visitaPersona);
        void DeleteVisitaPersona(VisitaPersona visitaPersona);
        void UpdateVisitaPersona(VisitaPersona visitaPersona);
        bool Save();
        Task<bool> SaveAsync();
    }
}
