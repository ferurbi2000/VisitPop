namespace VisitPop.Application.Interfaces.Visit
{
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;
    using VisitPop.Application.Dtos.Visit;

    public interface IVisitRepository
    {
        Task<PagedList<Visit>> GetVisitsAsync(VisitParametersDto visitParameters);
        Task<Visit> GetVisitAsync(int id);
        Visit GetVisit(int id);
        Task AddVisit(Visit visit);
        void DeleteVisit(Visit visit);
        void UpdateVisit(Visit visit);
        bool Save();
        Task<bool> SaveAsync();
    }
}
