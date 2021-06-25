namespace VisitPop.Application.Interfaces.Company
{
    using Application.Dtos.Company;
    using Application.Wrappers;
    using Domain.Entities;
    using System.Threading.Tasks;

    public interface ICompanyRepository
    {
        Task<PagedList<Company>> GetCompaniesAsync(CompanyParametersDto companyParameters);
        Task<Company> GetCompanyAsync(int companyId);
        Company GetCompany(int companyId);
        Task AddCompany(Company company);
        void DeleteCompany(Company company);
        void UpdateCompany(Company company);
        bool Save();
        Task<bool> SaveAsync();
    }
}
