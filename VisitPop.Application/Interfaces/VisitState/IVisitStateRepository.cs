namespace VisitPop.Application.Interfaces.VisitState
{
    using Application.Dtos.VisitState;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IVisitStateRepository
    {
        Task<PagedList<VisitState>> GetVisitStatesAsync(VisitStateParametersDto visitStateParametersDto);
        Task<VisitState> GetVisitStateAsync(int visitStateId);
        VisitState GetVisitState(int visitStateId);
        Task AddVisitState(VisitState visitState);
        void DeleteVisitState(VisitState visitState);
        void UpdateVisitState(VisitState visitState);
        bool Save();
        Task<bool> SaveAsync();
    }
}
