using System.Threading.Tasks;
using VisitPop.Application.Dtos.Office;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Office
{
    public interface IOfficeRepository
    {
        Task<PagingResponse<OfficeDto>> GetOfficesAsync(OfficeParametersDto officeParameters);
        Task<OfficeDto> GetOffice(int id);
        Task<OfficeDto> AddOffice(OfficeDto office);
        Task UpdateOffice(OfficeDto office);
        Task DeleteOffice(OfficeDto office);
    }
}
