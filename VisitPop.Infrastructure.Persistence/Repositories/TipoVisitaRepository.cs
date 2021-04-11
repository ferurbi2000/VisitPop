using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoVisita;
using VisitPop.Application.Interfaces.TipoVisita;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class TipoVisitaRepository : ITipoVisitaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TipoVisitaRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TipoVisita>> GetTipoVisitasAsync(TipoVisitaParametersDto tipoVisitaParameters)
        {
            if (tipoVisitaParameters == null)
            {
                throw new ArgumentNullException(nameof(tipoVisitaParameters));
            }

            var collection = _context.TipoVisitas
                as IQueryable<TipoVisita>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = tipoVisitaParameters.SortOrder ?? "TipoVisitaId",
                Filters = tipoVisitaParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TipoVisita>.CreateAsync(collection,
                tipoVisitaParameters.PageNumber,
                tipoVisitaParameters.PageSize);
        }

        public async Task<TipoVisita> GetTipoVisitaAsync(int id)
        {
            // include marker -- requires return _context.TipoVisitas as it's own line with no extra text -- do not delete this comment
            return await _context.TipoVisitas
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public TipoVisita GetTipoVisita(int id)
        {
            // include marker -- requires return _context.TipoVisitas as it's own line with no extra text -- do not delete this comment
            return _context.TipoVisitas
                .FirstOrDefault(t => t.Id == id);
        }

        public async Task AddTipoVisita(TipoVisita tipoVisita)
        {
            if (tipoVisita == null)
            {
                throw new ArgumentNullException(nameof(TipoVisita));
            }

            await _context.TipoVisitas.AddAsync(tipoVisita);
        }

        public void DeleteTipoVisita(TipoVisita tipoVisita)
        {
            if (tipoVisita == null)
            {
                throw new ArgumentNullException(nameof(TipoVisita));
            }

            _context.TipoVisitas.Remove(tipoVisita);
        }

        public void UpdateTipoVisita(TipoVisita tipoVisita)
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
