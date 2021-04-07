namespace VisitPop.Application.Interfaces.Empresa
{
    using Application.Dtos.Empresa;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface IEmpresaRepository
    {
        Task<PagedList<Empresa>> GetEmpresasAsync(EmpresaParametersDto empresaParameters);
        Task<Empresa> GetEmpresaAsync(int EmpresaId);
        Empresa GetEmpresa(int empresaId);
        Task AddEmpresa(Empresa empresa);
        void DeleteEmpresa(Empresa empresa);
        void UpdateEmpresa(Empresa empresa);
        bool Save();
        Task<bool> SaveAsync();
    }
}
