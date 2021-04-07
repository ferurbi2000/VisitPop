namespace VisitPop.Application.Interfaces.DepartamentoEmpleado
{
    using Application.Dtos.DepartamentoEmpleado;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IDepartamentoEmpleadoRepository
    {
        Task<PagedList<DepartamentoEmpleado>> GetDepartamentoEmpleadosAsync(DepartamentoEmpleadoParametersDto DepartamentoEmpleadoParameters);
        Task<DepartamentoEmpleado> GetDepartamentoEmpleadoAsync(int DepartamentoEmpleadoId);
        DepartamentoEmpleado GetDepartamentoEmpleado(int DepartamentoEmpleadoId);
        Task AddDepartamentoEmpleado(DepartamentoEmpleado DepartamentoEmpleado);
        void DeleteDepartamentoEmpleado(DepartamentoEmpleado DepartamentoEmpleado);
        void UpdateDepartamentoEmpleado(DepartamentoEmpleado DepartamentoEmpleado);
        bool Save();
        Task<bool> SaveAsync();
    }
}
