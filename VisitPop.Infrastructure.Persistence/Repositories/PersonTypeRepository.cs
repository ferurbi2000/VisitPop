using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.Application.Interfaces.PersonType;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class PersonTypeRepository : IPersonTypeRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public PersonTypeRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<PersonType>> GetPersonTypes(PersonTypeParametersDto personTypeParameters)
        {
            if (personTypeParameters == null)
            {
                throw new ArgumentNullException(nameof(personTypeParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.PersonTypes
                as IQueryable<PersonType>;

            var sieveModel = new SieveModel
            {
                Sorts = personTypeParameters.SortOrder ?? "Id",
                Filters = personTypeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<PersonType>.CreateAsync(collection,
                personTypeParameters.PageNumber,
                personTypeParameters.PageSize);
        }

        public async Task<PersonType> GetPersonTypeAsync(int id)
        {
            // include marker -- requires return _context.TipoPersonas as it's own line with no extra text -- do not delete this comment
            return await _context.PersonTypes
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public PersonType GetPersonType(int id)
        {
            // include marker -- requires return _context.TipoPersonas as it's own line with no extra text -- do not delete this comment
            return _context.PersonTypes
                .FirstOrDefault(t => t.Id == id);
        }

        public async Task AddPersonType(PersonType personType)
        {
            if (personType == null)
            {
                throw new ArgumentNullException(nameof(personType));
            }

            await _context.PersonTypes.AddAsync(personType);
        }

        public void DeletePersonType(PersonType personType)
        {
            if (personType == null)
            {
                throw new ArgumentNullException(nameof(personType));
            }

            _context.PersonTypes.Remove(personType);
        }

        public void UpdatePersonType(PersonType personType)
        {
            // no implementation for now
            _context.Entry(personType).State = EntityState.Modified;
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
