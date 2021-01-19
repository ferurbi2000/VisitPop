using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Interfaces.VisitPop;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private VisitPopDbContext _context;

        // Sieve is a simple, clean, and extensible framework for .NET Core that adds sorting, filtering, 
        //and pagination functionality out of the box. Most common use case would be for serving ASP.NET Core GET queries.
        private readonly SieveProcessor _sieveProcessor;

        public PersonRepository(VisitPopDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Person>> GetPersonsAsync(PersonParametersDto personParameters)
        {
            if (personParameters == null)
            {
                throw new ArgumentNullException(nameof(personParameters));
            }

            var collection = _context.People
                as IQueryable<Person>;
            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = personParameters.SortOrder ?? "PersonId",
                Filters = personParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Person>.CreateAsync(collection,
                personParameters.PageNumber,
                personParameters.PageSize);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            // include marker -- requires return _context.People as it's own line with no extra text -- do not delete this comment
            return await _context.People
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Person GetPerson(int id)
        {
            // include marker -- requires return _context.People as it's own line with no extra text -- do not delete this comment
            return _context.People.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddPerson(Person person)
        {
            if(person ==null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            await _context.People.AddAsync(person);
        }

        public void DeletePerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            _context.People.Remove(person);
        }

        public void UpdatePerson(Person person)
        {
            //  no implementation for now
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
