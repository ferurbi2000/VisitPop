using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Interfaces.VisitState;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VisitStateRepository : IVisitStateRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VisitStateRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<VisitState>> GetVisitStatesAsync(VisitStateParametersDto visitStateParametersDto)
        {
            if (visitStateParametersDto == null)
            {
                throw new ArgumentNullException(nameof(visitStateParametersDto));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.VisitStates
                as IQueryable<VisitState>;

            var sieveModel = new SieveModel
            {
                Sorts = visitStateParametersDto.SortOrder ?? "Id",
                Filters = visitStateParametersDto.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<VisitState>.CreateAsync(collection,
                visitStateParametersDto.PageNumber,
                visitStateParametersDto.PageSize);
        }

        public async Task<VisitState> GetVisitStateAsync(int visitStateId)
        {
            // include marker -- requires return _context.EstadoVisitas as it's own line with no extra text -- do not delete this comment
            return await _context.VisitStates
                .FirstOrDefaultAsync(e => e.Id == visitStateId);
        }

        public VisitState GetVisitState(int visitStateId)
        {
            // include marker -- requires return _context.EstadoVisitas as it's own line with no extra text -- do not delete this comment
            return _context.VisitStates
                .FirstOrDefault(e => e.Id == visitStateId);
        }

        public async Task AddVisitState(VisitState visitState)
        {
            if (visitState == null)
            {
                throw new ArgumentNullException(nameof(visitState));
            }

            await _context.VisitStates.AddAsync(visitState);
        }

        public void DeleteVisitState(VisitState visitState)
        {
            if (visitState == null)
            {
                throw new ArgumentNullException(nameof(visitState));
            }

            _context.VisitStates.Remove(visitState);
        }

        public void UpdateVisitState(VisitState visitState)
        {
            // no implementation for now
            _context.Entry(visitState).State = EntityState.Modified;
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
