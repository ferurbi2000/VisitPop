using System.Collections.Generic;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.DepartamentoEmpleado
{
    public interface IDepartamentoEmpleadoRepository
    {
        //Task<IEnumerable<DepartamentoEmpleadoDto>> GetDepartamentoEmpleadosAsync(int pageNumber, int pageSize);
        Task<PagingResponse<DepartamentoEmpleadoDto>> GetDepartamentoEmpleadosAsync(DepartamentoEmpleadoParametersDto departamentoEmpleadoParameters);
        Task<DepartamentoEmpleadoDto> GetDepartamentoEmpleado(int id);
        Task AddDepartamentoEmpleado(DepartamentoEmpleadoDto departamentoEmpleado);
        Task UpdateDepartamentoEmpleado(DepartamentoEmpleadoDto departamentoEmpleado);
        Task DeleteDepartamentoEmpleado(DepartamentoEmpleadoDto departamentoEmpleado);
    }
}
