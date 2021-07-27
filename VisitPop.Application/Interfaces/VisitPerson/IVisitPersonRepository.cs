namespace VisitPop.Application.Interfaces.VisitPerson
{
    using System.Threading.Tasks;
    using VisitPop.Application.Dtos.VisitPerson;
    using VisitPop.Application.Wrappers;
    using VisitPop.Domain.Entities;

    public interface IVisitPersonRepository
    {
        Task<PagedList<VisitPerson>> GetVisitPersonsAsync(VisitPersonParametersDto visitPersonParameters);
        Task<VisitPerson> GetVisitPersonAsync(int id);
        VisitPerson GetVisitPerson(int id);
        Task AddVisitPerson(VisitPerson visitPerson);
        void DeleteVisitPerson(VisitPerson visitPerson);
        void UpdateVisitPerson(VisitPerson visitPerson);
        bool Save();
        Task<bool> SaveAsync();
    }
}
