using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.EmployeeDepartment
{
    public interface IEmployeeDepartmentRepository
    {
        //Task<IEnumerable<DepartamentoEmpleadoDto>> GetDepartamentoEmpleadosAsync(int pageNumber, int pageSize);
        Task<PagingResponse<EmployeeDepartmentDto>> GetEmployeeDepartmentsAsync(EmployeeDepartmentParametersDto employeeDepartmentParameters);
        Task<EmployeeDepartmentDto> GetEmployeeDepartment(int id);
        Task<EmployeeDepartmentDto> AddEmployeeDepartment(EmployeeDepartmentDto employeeDepartment);
        Task UpdateEmployeeDepartment(EmployeeDepartmentDto employeeDepartment);
        Task DeleteEmployeeDepartment(EmployeeDepartmentDto employeeDepartment);
    }
}
