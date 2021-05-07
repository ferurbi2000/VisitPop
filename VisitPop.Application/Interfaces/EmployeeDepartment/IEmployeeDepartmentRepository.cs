namespace VisitPop.Application.Interfaces.EmployeeDepartment
{
    using Application.Dtos.EmployeeDepartment;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IEmployeeDepartmentRepository
    {
        Task<PagedList<EmployeeDepartment>> GetEmployeeDepartmentsAsync(EmployeeDepartmentParametersDto employeeDepartmentParameters);
        Task<EmployeeDepartment> GetEmployeeDepartmentAsync(int employeeDepartmentId);
        EmployeeDepartment GetEmployeeDepartment(int employeeDepartmentId);
        Task AddEmployeeDepartment(EmployeeDepartment employeeDepartment);
        void DeleteEmployeeDepartment(EmployeeDepartment employeeDepartment);
        void UpdateEmployeeDepartment(EmployeeDepartment employeeDepartment);        
        bool Save();
        Task<bool> SaveAsync();
    }
}
