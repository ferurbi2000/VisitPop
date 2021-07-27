using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitPerson;
using VisitPop.Application.Interfaces.VisitPerson;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class VisitPersonRepository : IVisitPersonRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public VisitPersonRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<VisitPerson>> GetVisitPersonsAsync(VisitPersonParametersDto visitPersonParameters)
        {
            if (visitPersonParameters == null)
            {
                throw new ArgumentNullException(nameof(visitPersonParameters));
            }

            var collection = _context.VisitPersons
                .Include(v => v.Visit)
                .Include(p => p.Person)
                .Include(t => t.VehicleType)
                as IQueryable<VisitPerson>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = visitPersonParameters.SortOrder ?? "VisitPersonId",
                Filters = visitPersonParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<VisitPerson>.CreateAsync(collection,
                visitPersonParameters.PageNumber,
                visitPersonParameters.PageSize);
        }

        public async Task<VisitPerson> GetVisitPersonAsync(int id)
        {
            // include marker -- requires return _context.VisitaPersonas as it's own line with no extra text -- do not delete this comment
            return await _context.VisitPersons
                .Include(v => v.Visit)
                .Include(p => p.Person)
                .Include(t => t.VehicleType)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public VisitPerson GetVisitPerson(int id)
        {
            // include marker -- requires return _context.VisitaPersonas as it's own line with no extra text -- do not delete this comment
            return _context.VisitPersons
                .Include(v => v.Visit)
                .Include(p => p.Person)
                .Include(t => t.VehicleType)
                .FirstOrDefault(v => v.Id == id);
        }

        public async Task AddVisitPerson(VisitPerson visitPerson)
        {
            if (visitPerson == null)
            {
                throw new ArgumentNullException(nameof(VisitPerson));
            }

            await _context.VisitPersons.AddAsync(visitPerson);
        }

        public void DeleteVisitPerson(VisitPerson visitPerson)
        {
            if (visitPerson == null)
            {
                throw new ArgumentNullException(nameof(VisitPerson));
            }

            _context.VisitPersons.Remove(visitPerson);
        }

        public void UpdateVisitPerson(VisitPerson visitPerson)
        {
            // no implementation for now
            _context.Entry(visitPerson).State = EntityState.Modified;
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
