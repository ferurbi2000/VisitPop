using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Persona;
using VisitPop.Application.Interfaces.Persona;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieve;

        public PersonaRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieve = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<Persona>> GetPersonasAsync(PersonaParametersDto personaParameters)
        {
            if (personaParameters == null)
                throw new ArgumentNullException(nameof(personaParameters));

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Personas
                .Include(t => t.TipoPersona)
                .Include(e => e.Empresa)
                as IQueryable<Persona>;

            var sieveModel = new SieveModel
            {
                Sorts = personaParameters.SortOrder ?? "Id",
                Filters = personaParameters.Filters
            };

            collection = _sieve.Apply(sieveModel, collection);

            return await PagedList<Persona>.CreateAsync(collection,
                personaParameters.PageNumber,
                personaParameters.PageSize);
        }

        public async Task<Persona> GetPersonaAsync(int id)
        {
            // include marker -- requires return _context.Personas as it's own line with no extra text -- do not delete this comment
            return await _context.Personas
                .Include(t => t.TipoPersona)
                .Include(e => e.Empresa)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Persona GetPersona(int id)
        {
            // include marker -- requires return _context.Personas as it's own line with no extra text -- do not delete this comment
            return _context.Personas
                .Include(t => t.TipoPersona)
                .Include(e => e.Empresa)
                .FirstOrDefault();
        }

        public async Task AddPersona(Persona persona)
        {
            if (persona == null)
                throw new ArgumentNullException(nameof(persona));

            await _context.Personas.AddAsync(persona);
        }

        public void DeletePersona(Persona persona)
        {
            if (persona == null)
                throw new ArgumentNullException(nameof(persona));

            _context.Personas.Remove(persona);
        }

        public void UpdatePersona(Persona persona)
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
