using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Visita;
using VisitPop.Application.Interfaces.Visita;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VisitaRepository : IVisitaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VisitaRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Visita>> GetVisitasAsync(VisitaParametersDto visitaParameters)
        {
            if (visitaParameters == null)
                throw new ArgumentNullException(nameof(visitaParameters));

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Visitas
                .Include(t => t.TipoVisita)
                .Include(e => e.Empleado)
                .Include(o => o.Oficina)
                .Include(p => p.PuntoControl)
                .Include(e => e.EstadoVisita)
                as IQueryable<Visita>;

            var sieveModel = new SieveModel
            {
                Sorts = visitaParameters.SortOrder ?? "Id",
                Filters = visitaParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Visita>.CreateAsync(collection,
                visitaParameters.PageNumber,
                visitaParameters.PageSize);
        }

        public async Task<Visita> GetVisitaAsync(int id)
        {
            // include marker -- requires return _context.Visitas as it's own line with no extra text -- do not delete this comment
            return await _context.Visitas
                .Include(t => t.TipoVisita)
                .Include(e => e.Empleado)
                .Include(o => o.Oficina)
                .Include(p => p.PuntoControl)
                .Include(e => e.EstadoVisita)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public Visita GetVisita(int id)
        {
            // include marker -- requires return _context.Visitas as it's own line with no extra text -- do not delete this comment
            return _context.Visitas
                .Include(t => t.TipoVisita)
                .Include(e => e.Empleado)
                .Include(o => o.Oficina)
                .Include(p => p.PuntoControl)
                .Include(e => e.EstadoVisita)
                .FirstOrDefault(v => v.Id == id);
        }

        public async Task AddVisita(Visita visita)
        {
            if (visita == null)
                throw new ArgumentNullException(nameof(visita));

            await _context.Visitas.AddAsync(visita);
        }

        public void DeleteVisita(Visita visita)
        {
            if (visita == null)
                throw new ArgumentNullException(nameof(visita));

            _context.Visitas.Remove(visita);
        }

        public void UpdateVisita(Visita visita)
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
