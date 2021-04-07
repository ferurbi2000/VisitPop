namespace VisitPop.Application.Interfaces.Empleado
{
    using Application.Dtos.Empleado;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IEmpleadoRepository
    {
        Task<PagedList<Empleado>> GetEmpleadosAsync(EmpleadoParametersDto EmpleadoParameters);
        Task<Empleado> GetEmpleadoAsync(int empleadoId);
        Empleado GetEmpleado(int empleadoId);
        Task AddEmpleado(Empleado empleado);
        void DeleteEmpleado(Empleado empleado);
        void UpdateEmpleado(Empleado empleado);
        bool Save();
        Task<bool> SaveAsync();

    }
}
