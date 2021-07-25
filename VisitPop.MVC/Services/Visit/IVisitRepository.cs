using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Visit
{
    public interface IVisitRepository
    {
        Task<PagingResponse<VisitDto>> GetVisitsAsync(VisitParametersDto visitParameters);
        Task<VisitDto> GetVisit(int id);
        Task<VisitDto> AddVisit(VisitDto visit);
        Task UpdateVisit(VisitDto visit);
        Task DeleteVisit(VisitDto visit);

        Task<PagingResponse<VisitTypeDto>> GetVisitTypesAsync(VisitTypeParametersDto visitTypeParameters);
        Task<PagingResponse<EmployeeDto>> GetEmployeesAsync(EmployeeParametersDto employeeParameters);
        Task<PagingResponse<OfficeDto>> GetOfficesAsync(OfficeParametersDto officeParameters);
        Task<PagingResponse<RegisterControlDto>> GetRegisterControlsAsync(RegisterControlParametersDto registerControlParameters);
        Task<PagingResponse<VisitStateDto>> GetVisitStatesAsync(VisitStateParametersDto visitStateParameters);
    }
}
