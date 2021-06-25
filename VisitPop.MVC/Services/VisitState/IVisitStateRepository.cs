using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.VisitState
{
    public interface IVisitStateRepository
    {
        Task<PagingResponse<VisitStateDto>> GetVisitStatesAsync(VisitStateParametersDto visitStateParameters);
        Task<VisitStateDto> GetVisitState(int id);
        Task<VisitStateDto> AddVisitState(VisitStateDto visitState);
        Task UpdateVisitState(VisitStateDto visitState);
        Task DeleteVisitState(VisitStateDto visitState);
    }
}
