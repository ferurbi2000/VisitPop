using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Empresa;
using VisitPop.Application.Interfaces.Empresa;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public EmpresaRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Empresa>> GetEmpresasAsync(EmpresaParametersDto empresaParameters)
        {
            if (empresaParameters == null)
            {
                throw new ArgumentNullException(nameof(empresaParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Empresas
                as IQueryable<Empresa>;

            var sieveModel = new SieveModel
            {
                Sorts = empresaParameters.SortOrder ?? "Id",
                Filters = empresaParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Empresa>.CreateAsync(collection,
                empresaParameters.PageNumber,
                empresaParameters.PageSize);
        }

        public async Task<Empresa> GetEmpresaAsync(int EmpresaId)
        {
            // include marker -- requires return _context.Empresas as it's own line with no extra text -- do not delete this comment
            return await _context.Empresas
                .FirstOrDefaultAsync(e => e.Id == EmpresaId);
        }

        public Empresa GetEmpresa(int empresaId)
        {
            // include marker -- requires return _context.Empresas as it's own line with no extra text -- do not delete this comment
            return _context.Empresas
                .FirstOrDefault(e => e.Id == empresaId);
        }

        public async Task AddEmpresa(Empresa empresa)
        {
            if (empresa == null)
            {
                throw new ArgumentNullException(nameof(empresa));
            }

            await _context.Empresas.AddAsync(empresa);
        }

        public void DeleteEmpresa(Empresa empresa)
        {
            if (empresa == null)
            {
                throw new ArgumentNullException(nameof(empresa));
            }

            _context.Empresas.Remove(empresa);
        }

        public void UpdateEmpresa(Empresa empresa)
        {
            // no implementation for now
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
