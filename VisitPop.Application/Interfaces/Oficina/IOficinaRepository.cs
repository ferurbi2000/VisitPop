namespace VisitPop.Application.Interfaces.Oficina
{
    using Domain.Entities;
    using Application.Dtos.Oficina;
    using Application.Wrappers;
    using System.Threading.Tasks;

    public interface IOficinaRepository
    {
        Task<PagedList<Oficina>> GetOficinasAsync(OficinaParametersDto oficinaParametersDto);
        Task<Oficina> GetOficinaAsync(int Id);
        Oficina GetOficina(int Id);
        Task AddOficina(Oficina oficina);
        void DeleteOficina(Oficina oficina);
        void UpdateOficina(Oficina oficina);
        bool Save();
        Task<bool> SaveAsync();
    }
}
