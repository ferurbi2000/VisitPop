using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.VisitType
{
    public interface IVisitTypeRepository
    {
        Task<PagingResponse<VisitTypeDto>> GetVisitTypesAsync(VisitTypeParametersDto visitTypeParameters);
        Task<VisitTypeDto> GetVisitType(int id);
        Task<VisitTypeDto> AddVisitType(VisitTypeDto visitType);
        Task UpdateVisitType(VisitTypeDto visitType);
        Task DeleteVisitType(VisitTypeDto visitType);
    }
}
