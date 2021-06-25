using System.Threading.Tasks;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.RegisterControl
{
    public interface IRegisterControlRepository
    {
        Task<PagingResponse<RegisterControlDto>> GetRegisterControlsAsync(RegisterControlParametersDto registerControlParameters);
        Task<RegisterControlDto> GetRegisterControl(int id);
        Task<RegisterControlDto> AddRegisterControl(RegisterControlDto registerControl);
        Task UpdateRegisterControl(RegisterControlDto registerControl);
        Task DeleteRegisterControl(RegisterControlDto registerControl);
    }
}
