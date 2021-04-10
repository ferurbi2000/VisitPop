using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoPersona;
using VisitPop.Application.Interfaces.TipoPersona;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class TipoPersonaRepository : ITipoPersonaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TipoPersonaRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TipoPersona>> GetTipoPersonas(TipoPersonaParametersDto tipoPersonaParameters)
        {
            if (tipoPersonaParameters == null)
            {
                throw new ArgumentNullException(nameof(tipoPersonaParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.TipoPersonas
                as IQueryable<TipoPersona>;

            var sieveModel = new SieveModel
            {
                Sorts = tipoPersonaParameters.SortOrder ?? "Id",
                Filters = tipoPersonaParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TipoPersona>.CreateAsync(collection,
                tipoPersonaParameters.PageNumber,
                tipoPersonaParameters.PageSize);
        }

        public async Task<TipoPersona> GetTipoPersonaAsync(int id)
        {
            // include marker -- requires return _context.TipoPersonas as it's own line with no extra text -- do not delete this comment
            return await _context.TipoPersonas
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public TipoPersona GetTipoPersona(int id)
        {
            // include marker -- requires return _context.TipoPersonas as it's own line with no extra text -- do not delete this comment
            return _context.TipoPersonas
                .FirstOrDefault(t => t.Id == id);
        }

        public async Task AddTipoPersona(TipoPersona tipoPersona)
        {
            if (tipoPersona == null)
            {
                throw new ArgumentNullException(nameof(tipoPersona));
            }

            await _context.TipoPersonas.AddAsync(tipoPersona);
        }

        public void DeleteTipoPersona(TipoPersona tipoPersona)
        {
            if (tipoPersona == null)
            {
                throw new ArgumentNullException(nameof(tipoPersona));
            }

            _context.TipoPersonas.Remove(tipoPersona);
        }

        public void UpdateTipoPersona(TipoPersona tipoPersona)
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
