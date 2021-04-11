namespace VisitPop.Application.Interfaces.TipoVisita
{
    using Application.Dtos.TipoVisita;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface ITipoVisitaRepository
    {
        Task<PagedList<TipoVisita>> GetTipoVisitasAsync(TipoVisitaParametersDto tipoVisitaParameters);
        Task<TipoVisita> GetTipoVisitaAsync(int id);
        TipoVisita GetTipoVisita(int id);
        Task AddTipoVisita(TipoVisita tipoVisita);
        void DeleteTipoVisita(TipoVisita tipoVisita);
        void UpdateTipoVisita(TipoVisita tipoVisita);
        bool Save();
        Task<bool> SaveAsync();
    }
}
