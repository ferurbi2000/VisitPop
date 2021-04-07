using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PuntoControl;
using VisitPop.Application.Interfaces.PuntoControl;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class PuntoControlRepository : IPuntoControlRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public PuntoControlRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<PuntoControl>> GetPuntoControlesAsync(PuntoControlParametersDto puntoControlParameters)
        {
            if (puntoControlParameters == null)
            {
                throw new ArgumentNullException(nameof(puntoControlParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.PuntoControles
                as IQueryable<PuntoControl>;

            var sieveModel = new SieveModel
            {
                Sorts = puntoControlParameters.SortOrder ?? "Id",
                Filters = puntoControlParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<PuntoControl>.CreateAsync(collection,
                puntoControlParameters.PageNumber,
                puntoControlParameters.PageSize);
        }

        public async Task<PuntoControl> GetPuntoControlAsync(int id)
        {
            // include marker -- requires return _context.PuntoControls as it's own line with no extra text -- do not delete this comment
            return await _context.PuntoControles
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public PuntoControl GetPuntoControl(int id)
        {
            // include marker -- requires return _context.PuntoControls as it's own line with no extra text -- do not delete this comment
            return _context.PuntoControles
                .FirstOrDefault(p => p.Id == id);
        }

        public async Task AddPuntoControl(PuntoControl puntoControl)
        {
            if (puntoControl == null)
            {
                throw new ArgumentNullException(nameof(puntoControl));
            }

            await _context.PuntoControles.AddAsync(puntoControl);
        }

        public void DeletePuntoControl(PuntoControl puntoControl)
        {
            if (puntoControl == null)
            {
                throw new ArgumentNullException(nameof(puntoControl));
            }

            _context.PuntoControles.Remove(puntoControl);
        }

        public void UpdatePuntoControl(PuntoControl puntoControl)
        {
            // no implementation for now
        }

        public bool Saves()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
