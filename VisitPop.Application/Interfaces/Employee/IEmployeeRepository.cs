namespace VisitPop.Application.Interfaces.Employee
{
    using Application.Dtos.Employee;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(EmployeeParametersDto employeeParameters);
        Task<Employee> GetEmployeeAsync(int employeeId);
        Employee GetEmployee(int employeeId);
        Task AddEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        bool Save();
        Task<bool> SaveAsync();

    }
}
