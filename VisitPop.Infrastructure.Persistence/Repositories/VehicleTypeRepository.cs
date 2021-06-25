using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.Application.Interfaces.VehicleType;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VehicleTypeRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<VehicleType>> GetVehicleTypesAsync(VehicleTypeParametersDto vehicleTypeParameters)
        {
            if (vehicleTypeParameters == null)
                throw new ArgumentNullException(nameof(vehicleTypeParameters));

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.VehicleTypes
                as IQueryable<VehicleType>;

            var sieveModel = new SieveModel
            {
                Sorts = vehicleTypeParameters.SortOrder ?? "Id",
                Filters = vehicleTypeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<VehicleType>.CreateAsync(collection,
                vehicleTypeParameters.PageNumber,
                vehicleTypeParameters.PageSize);
        }

        public async Task<VehicleType> GetVehicleTypeAsync(int id)
        {
            // include marker -- requires return _context.TipoVehiculos as it's own line with no extra text -- do not delete this comment
            return await _context.VehicleTypes
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public VehicleType GetVehicleType(int id)
        {
            // include marker -- requires return _context.TipoVehiculos as it's own line with no extra text -- do not delete this comment
            return _context.VehicleTypes
                .FirstOrDefault(t => t.Id == id);
        }

        public async Task AddVehicleType(VehicleType vehicleType)
        {
            if (vehicleType == null)
                throw new ArgumentNullException(nameof(vehicleType));

            await _context.VehicleTypes.AddAsync(vehicleType);
        }

        public void DeleteVehicleType(VehicleType vehicleType)
        {
            if (vehicleType == null)
                throw new ArgumentNullException(nameof(vehicleType));

            _context.VehicleTypes.Remove(vehicleType);
        }

        public void UpdateVehicleType(VehicleType vehicleType)
        {
            // no implementation for now
            _context.Entry(vehicleType).State = EntityState.Modified;
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
