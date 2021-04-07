using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EstadoVisita;
using VisitPop.Application.Interfaces.EstadoVisita;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class EstadoVisitaRepository : IEstadoVisitaRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public EstadoVisitaRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<EstadoVisita>> GetEstadoVisitasAsync(EstadoVisitaParametersDto estadoVisitaParametersDto)
        {
            if (estadoVisitaParametersDto == null)
            {
                throw new ArgumentNullException(nameof(estadoVisitaParametersDto));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.EstadoVisitas
                as IQueryable<EstadoVisita>;

            var sieveModel = new SieveModel
            {
                Sorts = estadoVisitaParametersDto.SortOrder ?? "Id",
                Filters = estadoVisitaParametersDto.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<EstadoVisita>.CreateAsync(collection,
                estadoVisitaParametersDto.PageNumber,
                estadoVisitaParametersDto.PageSize);
        }

        public async Task<EstadoVisita> GetEstadoVisitaAsync(int estadoVisitaId)
        {
            // include marker -- requires return _context.EstadoVisitas as it's own line with no extra text -- do not delete this comment
            return await _context.EstadoVisitas
                .FirstOrDefaultAsync(e => e.Id == estadoVisitaId);
        }

        public EstadoVisita GetEstadoVisita(int estadoVisitaId)
        {
            // include marker -- requires return _context.EstadoVisitas as it's own line with no extra text -- do not delete this comment
            return _context.EstadoVisitas
                .FirstOrDefault(e => e.Id == estadoVisitaId);
        }

        public async Task AddEstadoVisita(EstadoVisita estadoVisita)
        {
            if (estadoVisita == null)
            {
                throw new ArgumentNullException(nameof(estadoVisita));
            }

            await _context.EstadoVisitas.AddAsync(estadoVisita);
        }

        public void DeleteEstadoVisita(EstadoVisita estadoVisita)
        {
            if (estadoVisita == null)
            {
                throw new ArgumentNullException(nameof(estadoVisita));
            }

            _context.EstadoVisitas.Remove(estadoVisita);
        }

        public void UpdateEstadoVisita(EstadoVisita estadoVisita)
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
