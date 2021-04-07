using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Observacion;
using VisitPop.Application.Interfaces.Observacion;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class ObservacionRepository : IObservacionRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public ObservacionRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));

            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Observacion>> GetObservacionesAsync(ObservacionParametersDto observacionParameters)
        {
            if (observacionParameters == null)
            {
                throw new ArgumentNullException(nameof(observacionParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Observaciones
                .Include(v => v.Visita)
                as IQueryable<Observacion>;

            var sieveModel = new SieveModel
            {
                Sorts = observacionParameters.SortOrder ?? "Id",
                Filters = observacionParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Observacion>.CreateAsync(collection,
                observacionParameters.PageNumber,
                observacionParameters.PageSize);
        }

        public async Task<Observacion> GetObservacionAsync(int id)
        {
            // include marker -- requires return _context.Observacions as it's own line with no extra text -- do not delete this comment
            return await _context.Observaciones
                .Include(v => v.Visita)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public Observacion GetObservacion(int id)
        {
            // include marker -- requires return _context.Observacions as it's own line with no extra text -- do not delete this comment
            return _context.Observaciones
                .Include(v => v.Visita)
                .FirstOrDefault(o => o.Id == id);
        }

        public async Task AddObservacion(Observacion observacion)
        {
            if (observacion == null)
            {
                throw new ArgumentNullException(nameof(observacion));
            }

            await _context.Observaciones.AddAsync(observacion);
        }

        public void DeleteObservacion(Observacion observacion)
        {
            if (observacion == null)
            {
                throw new ArgumentNullException(nameof(observacion));
            }
            _context.Observaciones.Remove(observacion);
        }

        public void UpdateObservacion(Observacion observacion)
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
