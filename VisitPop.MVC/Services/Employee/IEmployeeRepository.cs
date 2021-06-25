using System.Collections.Generic;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Employee
{
    public interface IEmployeeRepository
    {
        Task<PagingResponse<EmployeeDto>> GetEmployeesAsync(EmployeeParametersDto employeeParameters);
        Task<EmployeeDto> GetEmployee(int id);
        Task<EmployeeDto> AddEmployee(EmployeeDto employee);
        Task UpdateEmployee(EmployeeDto employee);
        Task DeleteEmployee(EmployeeDto employee);

        Task<PagingResponse<EmployeeDepartmentDto>> GetEmployeeDepartmentsAsync(EmployeeDepartmentParametersDto employeeDepartmentParameters);
    }
}
