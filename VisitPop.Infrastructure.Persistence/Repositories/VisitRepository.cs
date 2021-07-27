using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Application.Interfaces.Visit;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VisitRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Visit>> GetVisitsAsync(VisitParametersDto visitParameters)
        {
            if (visitParameters == null)
                throw new ArgumentNullException(nameof(visitParameters));

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Visits
                .Include(t => t.VisitType)
                .Include(e => e.Employee)
                .Include(o => o.Office)
                .Include(p => p.RegisterControl)
                .Include(e => e.VisitState)
                as IQueryable<Visit>;

            var sieveModel = new SieveModel
            {
                Sorts = visitParameters.SortOrder ?? "Id",
                Filters = visitParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Visit>.CreateAsync(collection,
                visitParameters.PageNumber,
                visitParameters.PageSize);
        }

        public async Task<Visit> GetVisitAsync(int id)
        {
            // include marker -- requires return _context.Visitas as it's own line with no extra text -- do not delete this comment
            return await _context.Visits                
                .Include(t => t.VisitType)
                .Include(e => e.Employee)
                .Include(o => o.Office)
                .Include(p => p.RegisterControl)
                .Include(e => e.VisitState)                
                .FirstOrDefaultAsync(v => v.Id == id);


        }

        public Visit GetVisit(int id)
        {
            // include marker -- requires return _context.Visitas as it's own line with no extra text -- do not delete this comment
            return _context.Visits
                .Include(t => t.VisitType)
                .Include(e => e.Employee)
                .Include(o => o.Office)
                .Include(p => p.RegisterControl)
                .Include(e => e.VisitState)
                .FirstOrDefault(v => v.Id == id);
        }

        public async Task AddVisit(Visit visit)
        {
            if (visit == null)
                throw new ArgumentNullException(nameof(visit));

            await _context.Visits.AddAsync(visit);
        }

        public void DeleteVisit(Visit visit)
        {
            if (visit == null)
                throw new ArgumentNullException(nameof(visit));

            _context.Visits.Remove(visit);
        }

        public void UpdateVisit(Visit visit)
        {
            // no implementation for now
            _context.Entry(visit).State = EntityState.Modified;
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
