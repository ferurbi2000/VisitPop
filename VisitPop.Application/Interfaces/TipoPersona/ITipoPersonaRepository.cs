namespace VisitPop.Application.Interfaces.TipoPersona
{
    using Domain.Entities;
    using global::VisitPop.Application.Dtos.TipoPersona;
    using global::VisitPop.Application.Wrappers;
    using System.Threading.Tasks;

    public interface ITipoPersonaRepository
    {
        Task<PagedList<TipoPersona>> GetTipoPersonas(TipoPersonaParametersDto tipoPersonaParameters);
        Task<TipoPersona> GetTipoPersonaAsync(int id);
        TipoPersona GetTipoPersona(int id);
        Task AddTipoPersona(TipoPersona tipoPersona);
        void DeleteTipoPersona(TipoPersona tipoPersona);
        void UpdateTipoPersona(TipoPersona tipoPersona);
        bool Save();
        Task<bool> SaveAsync();
    }
}
