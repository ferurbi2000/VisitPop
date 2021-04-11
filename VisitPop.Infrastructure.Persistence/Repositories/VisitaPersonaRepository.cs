using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitaPersona;
using VisitPop.Application.Interfaces.VisitaPersona;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VisitaPersonaRepository : IVisitaPersonaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VisitaPersonaRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<VisitaPersona>> GetVisitaPersonasAsync(VisitaPersonaParametersDto visitaPersonaParameters)
        {
            if (visitaPersonaParameters == null)
            {
                throw new ArgumentNullException(nameof(visitaPersonaParameters));
            }

            var collection = _context.VisitaPersonas
                .Include(v => v.Visita)
                .Include(p => p.Persona)
                .Include(t => t.TipoVehiculo)
                as IQueryable<VisitaPersona>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = visitaPersonaParameters.SortOrder ?? "VisitaPersonaId",
                Filters = visitaPersonaParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<VisitaPersona>.CreateAsync(collection,
                visitaPersonaParameters.PageNumber,
                visitaPersonaParameters.PageSize);
        }

        public async Task<VisitaPersona> GetVisitaPersonaAsync(int id)
        {
            // include marker -- requires return _context.VisitaPersonas as it's own line with no extra text -- do not delete this comment
            return await _context.VisitaPersonas
                .Include(v => v.Visita)
                .Include(p => p.Persona)
                .Include(t => t.TipoVehiculo)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public VisitaPersona GetVisitaPersona(int id)
        {
            // include marker -- requires return _context.VisitaPersonas as it's own line with no extra text -- do not delete this comment
            return _context.VisitaPersonas
                .Include(v => v.Visita)
                .Include(p => p.Persona)
                .Include(t => t.TipoVehiculo)
                .FirstOrDefault(v => v.Id == id);
        }

        public async Task AddVisitaPersona(VisitaPersona visitaPersona)
        {
            if (visitaPersona == null)
            {
                throw new ArgumentNullException(nameof(VisitaPersona));
            }

            await _context.VisitaPersonas.AddAsync(visitaPersona);
        }

        public void DeleteVisitaPersona(VisitaPersona visitaPersona)
        {
            if (visitaPersona == null)
            {
                throw new ArgumentNullException(nameof(VisitaPersona));
            }

            _context.VisitaPersonas.Remove(visitaPersona);
        }

        public void UpdateVisitaPersona(VisitaPersona visitaPersona)
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
