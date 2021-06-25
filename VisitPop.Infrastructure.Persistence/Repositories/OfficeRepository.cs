using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Interfaces.Office;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public OfficeRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Office>> GetOfficesAsync(OfficeParametersDto officeParametersDto)
        {
            if (officeParametersDto == null)
            {
                throw new ArgumentNullException(nameof(officeParametersDto));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Offices
                as IQueryable<Office>;

            var sieveModel = new SieveModel
            {
                Sorts = officeParametersDto.SortOrder ?? "Id",
                Filters = officeParametersDto.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Office>.CreateAsync(collection,
                officeParametersDto.PageNumber,
                officeParametersDto.PageSize);
        }

        public async Task<Office> GetOfficeAsync(int Id)
        {
            // include marker -- requires return _context.Oficinas as it's own line with no extra text -- do not delete this comment
            return await _context.Offices
                .FirstOrDefaultAsync(o => o.Id == Id);
        }

        public Office GetOffice(int Id)
        {
            // include marker -- requires return _context.Oficinas as it's own line with no extra text -- do not delete this comment
            return _context.Offices
                .FirstOrDefault(o => o.Id == Id);
        }

        public async Task AddOffice(Office office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(Office));
            }

            await _context.Offices.AddAsync(office);
        }

        public void DeleteOffice(Office office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(Office));
            }

            _context.Offices.Remove(office);
        }

        public void UpdateOffice(Office office)
        {
            // no implementation for now
            _context.Entry(office).State = EntityState.Modified;
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
