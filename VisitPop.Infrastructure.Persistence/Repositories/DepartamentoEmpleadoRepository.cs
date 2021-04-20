using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.Application.Interfaces.DepartamentoEmpleado;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class DepartamentoEmpleadoRepository : IDepartamentoEmpleadoRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public DepartamentoEmpleadoRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<DepartamentoEmpleado>> GetDepartamentoEmpleadosAsync(DepartamentoEmpleadoParametersDto DepartamentoEmpleadoParameters)
        {
            if (DepartamentoEmpleadoParameters == null)
            {
                throw new ArgumentNullException(nameof(DepartamentoEmpleadoParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.DepartamentoEmpleados
                as IQueryable<DepartamentoEmpleado>;

            var sieveModel = new SieveModel
            {
                Sorts = DepartamentoEmpleadoParameters.SortOrder ?? "Id",
                Filters = DepartamentoEmpleadoParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<DepartamentoEmpleado>.CreateAsync(collection,
                DepartamentoEmpleadoParameters.PageNumber,
                DepartamentoEmpleadoParameters.PageSize);
        }

        public async Task<DepartamentoEmpleado> GetDepartamentoEmpleadoAsync(int DepartamentoEmpleadoId)
        {
            // include marker -- requires return _context.DepartamentoEmpleados as it's own line with no extra text -- do not delete this comment
            return await _context.DepartamentoEmpleados
                .FirstOrDefaultAsync(d => d.Id == DepartamentoEmpleadoId);
        }

        public DepartamentoEmpleado GetDepartamentoEmpleado(int DepartamentoEmpleadoId)
        {
            // include marker -- requires return _context.DepartamentoEmpleados as it's own line with no extra text -- do not delete this comment
            return _context.DepartamentoEmpleados
                .FirstOrDefault(d => d.Id == DepartamentoEmpleadoId);
        }

        public async Task AddDepartamentoEmpleado(DepartamentoEmpleado DepartamentoEmpleado)
        {
            if (DepartamentoEmpleado == null)
            {
                throw new ArgumentNullException(nameof(DepartamentoEmpleado));
            }

            await _context.DepartamentoEmpleados.AddAsync(DepartamentoEmpleado);
        }

        public void DeleteDepartamentoEmpleado(DepartamentoEmpleado DepartamentoEmpleado)
        {
            if (DepartamentoEmpleado == null)
            {
                throw new ArgumentNullException(nameof(DepartamentoEmpleado));
            }

            _context.DepartamentoEmpleados.Remove(DepartamentoEmpleado);

        }

        public void UpdateDepartamentoEmpleado(DepartamentoEmpleado DepartamentoEmpleado)
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
