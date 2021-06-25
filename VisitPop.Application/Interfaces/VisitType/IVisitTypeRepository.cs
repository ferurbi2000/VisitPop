namespace VisitPop.Application.Interfaces.VisitType
{
    using Application.Dtos.VisitType;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IVisitTypeRepository
    {
        Task<PagedList<VisitType>> GetVisitTypesAsync(VisitTypeParametersDto visitTypeParameters);
        Task<VisitType> GetVisitTypeAsync(int id);
        VisitType GetVisitType(int id);
        Task AddVisitType(VisitType visitType);
        void DeleteVisitType(VisitType visitType);
        void UpdateVisitType(VisitType visitType);
        bool Save();
        Task<bool> SaveAsync();
    }
}
