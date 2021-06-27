using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Interfaces.Person;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieve;

        public PersonRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieve = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<Person>> GetPersonsAsync(PersonParametersDto personParameters)
        {
            if (personParameters == null)
                throw new ArgumentNullException(nameof(personParameters));

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Persons
                .Include(t => t.PersonTypes)
                .Include(e => e.Companies)
                as IQueryable<Person>;

            var sieveModel = new SieveModel
            {
                Sorts = personParameters.SortOrder ?? "Id",
                Filters = personParameters.Filters
            };

            collection = _sieve.Apply(sieveModel, collection);

            return await PagedList<Person>.CreateAsync(collection,
                personParameters.PageNumber,
                personParameters.PageSize);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            // include marker -- requires return _context.Personas as it's own line with no extra text -- do not delete this comment
            return await _context.Persons
                .Include(t => t.PersonTypes)
                .Include(e => e.Companies)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Person GetPerson(int id)
        {
            // include marker -- requires return _context.Personas as it's own line with no extra text -- do not delete this comment
            return _context.Persons
                .Include(t => t.PersonTypes)
                .Include(e => e.Companies)
                .FirstOrDefault();
        }

        public async Task AddPerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            await _context.Persons.AddAsync(person);
        }

        public void DeletePerson(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            _context.Persons.Remove(person);
        }

        public void UpdatePerson(Person person)
        {
            // no implementation for now
            _context.Entry(person).State = EntityState.Modified;
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
