using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.Application.Interfaces.VisitType;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VisitTypeRepository : IVisitTypeRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VisitTypeRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<VisitType>> GetVisitTypesAsync(VisitTypeParametersDto visitTypeParameters)
        {
            if (visitTypeParameters == null)
            {
                throw new ArgumentNullException(nameof(visitTypeParameters));
            }

            var collection = _context.VisitTypes
                as IQueryable<VisitType>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = visitTypeParameters.SortOrder ?? "Id",
                Filters = visitTypeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<VisitType>.CreateAsync(collection,
                visitTypeParameters.PageNumber,
                visitTypeParameters.PageSize);
        }

        public async Task<VisitType> GetVisitTypeAsync(int id)
        {
            // include marker -- requires return _context.TipoVisitas as it's own line with no extra text -- do not delete this comment
            return await _context.VisitTypes
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public VisitType GetVisitType(int id)
        {
            // include marker -- requires return _context.TipoVisitas as it's own line with no extra text -- do not delete this comment
            return _context.VisitTypes
                .FirstOrDefault(t => t.Id == id);
        }

        public async Task AddVisitType(VisitType visitType)
        {
            if (visitType == null)
            {
                throw new ArgumentNullException(nameof(VisitType));
            }

            await _context.VisitTypes.AddAsync(visitType);
        }

        public void DeleteVisitType(VisitType visitType)
        {
            if (visitType == null)
            {
                throw new ArgumentNullException(nameof(VisitType));
            }

            _context.VisitTypes.Remove(visitType);
        }

        public void UpdateVisitType(VisitType visitType)
        {
            // no implementation for now
            _context.Entry(visitType).State = EntityState.Modified;
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
