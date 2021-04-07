using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Oficina;
using VisitPop.Application.Interfaces.Oficina;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class OficinaRepository : IOficinaRepository
    {
        private VisitPopDbContext _contex;
        private readonly SieveProcessor _sieveProcessor;

        public OficinaRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _contex = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Oficina>> GetOficinasAsync(OficinaParametersDto oficinaParametersDto)
        {
            if (oficinaParametersDto == null)
            {
                throw new ArgumentNullException(nameof(oficinaParametersDto));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _contex.Oficinas
                as IQueryable<Oficina>;

            var sieveModel = new SieveModel
            {
                Sorts = oficinaParametersDto.SortOrder ?? "Id",
                Filters = oficinaParametersDto.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Oficina>.CreateAsync(collection,
                oficinaParametersDto.PageNumber,
                oficinaParametersDto.PageSize);
        }

        public async Task<Oficina> GetOficinaAsync(int Id)
        {
            // include marker -- requires return _context.Oficinas as it's own line with no extra text -- do not delete this comment
            return await _contex.Oficinas
                .FirstOrDefaultAsync(o => o.Id == Id);
        }

        public Oficina GetOficina(int Id)
        {
            // include marker -- requires return _context.Oficinas as it's own line with no extra text -- do not delete this comment
            return _contex.Oficinas
                .FirstOrDefault(o => o.Id == Id);
        }

        public async Task AddOficina(Oficina oficina)
        {
            if (oficina == null)
            {
                throw new ArgumentNullException(nameof(Oficina));
            }

            await _contex.Oficinas.AddAsync(oficina);
        }

        public void DeleteOficina(Oficina oficina)
        {
            if (oficina == null)
            {
                throw new ArgumentNullException(nameof(Oficina));
            }

            _contex.Oficinas.Remove(oficina);
        }

        public void UpdateOficina(Oficina oficina)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return _contex.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _contex.SaveChangesAsync() > 0;
        }
    }
}
