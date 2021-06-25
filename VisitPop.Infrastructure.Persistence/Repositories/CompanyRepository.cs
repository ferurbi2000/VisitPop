using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Interfaces.Company;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public CompanyRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyParametersDto companyParameters)
        {
            if (companyParameters == null)
            {
                throw new ArgumentNullException(nameof(companyParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Companies
                as IQueryable<Company>;

            var sieveModel = new SieveModel
            {
                Sorts = companyParameters.SortOrder ?? "Id",
                Filters = companyParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Company>.CreateAsync(collection,
                companyParameters.PageNumber,
                companyParameters.PageSize);
        }

        public async Task<Company> GetCompanyAsync(int companyId)
        {
            // include marker -- requires return _context.Empresas as it's own line with no extra text -- do not delete this comment
            return await _context.Companies
                .FirstOrDefaultAsync(e => e.Id == companyId);
        }

        public Company GetCompany(int companyId)
        {
            // include marker -- requires return _context.Empresas as it's own line with no extra text -- do not delete this comment
            return _context.Companies
                .FirstOrDefault(e => e.Id == companyId);
        }

        public async Task AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            await _context.Companies.AddAsync(company);
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _context.Companies.Remove(company);
        }

        public void UpdateCompany(Company company)
        {
            // no implementation for now
            _context.Entry(company).State = EntityState.Modified;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
