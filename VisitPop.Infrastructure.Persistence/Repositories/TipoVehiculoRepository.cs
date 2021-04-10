using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.TipoVehiculo;
using VisitPop.Application.Interfaces.TipoVehiculo;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class TipoVehiculoRepository : ITipoVehiculoRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TipoVehiculoRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<TipoVehiculo>> GetTipoVehiculosAsync(TipoVehiculoParametersDto tipoVehiculoParameters)
        {
            if (tipoVehiculoParameters == null)
                throw new ArgumentNullException(nameof(tipoVehiculoParameters));

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.TipoVehiculos
                as IQueryable<TipoVehiculo>;

            var sieveModel = new SieveModel
            {
                Sorts = tipoVehiculoParameters.SortOrder ?? "Id",
                Filters = tipoVehiculoParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TipoVehiculo>.CreateAsync(collection,
                tipoVehiculoParameters.PageNumber,
                tipoVehiculoParameters.PageSize);
        }

        public async Task<TipoVehiculo> GetTipoVehiculoAsync(int id)
        {
            // include marker -- requires return _context.TipoVehiculos as it's own line with no extra text -- do not delete this comment
            return await _context.TipoVehiculos
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public TipoVehiculo GetTipoVehiculo(int id)
        {
            // include marker -- requires return _context.TipoVehiculos as it's own line with no extra text -- do not delete this comment
            return _context.TipoVehiculos
                .FirstOrDefault(t => t.Id == id);
        }

        public async Task AddTipoVehiculo(TipoVehiculo tipoVehiculo)
        {
            if (tipoVehiculo == null)
                throw new ArgumentNullException(nameof(tipoVehiculo));

            await _context.TipoVehiculos.AddAsync(tipoVehiculo);
        }

        public void DeleteTipoVehiculo(TipoVehiculo tipoVehiculo)
        {
            if (tipoVehiculo == null)
                throw new ArgumentNullException(nameof(tipoVehiculo));

            _context.TipoVehiculos.Remove(tipoVehiculo);
        }

        public void UpdateTipoVehiculo(TipoVehiculo tipoVehiculo)
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
