using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Company
{
    public interface ICompanyRepository
    {
        Task<PagingResponse<CompanyDto>> GetCompaniesAsync(CompanyParametersDto companyParameters);
        Task<CompanyDto> GetCompany(int id);
        Task<CompanyDto> AddCompany(CompanyDto company);
        Task UpdateCompany(CompanyDto company);
        Task DeleteCompany(CompanyDto company);
    }
}
