using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Interfaces.RegisterControl;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class RegisterControlRepository : IRegisterControlRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public RegisterControlRepository(VisitPopDbContext context,
            SieveProcessor sieve)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieve
                ?? throw new ArgumentNullException(nameof(sieve));
        }

        public async Task<PagedList<RegisterControl>> GetRegisterControlsAsync(RegisterControlParametersDto registerControlParameters)
        {
            if (registerControlParameters == null)
            {
                throw new ArgumentNullException(nameof(registerControlParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.RegisterControls
                as IQueryable<RegisterControl>;

            var sieveModel = new SieveModel
            {
                Sorts = registerControlParameters.SortOrder ?? "Id",
                Filters = registerControlParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<RegisterControl>.CreateAsync(collection,
                registerControlParameters.PageNumber,
                registerControlParameters.PageSize);
        }

        public async Task<RegisterControl> GetRegisterControlAsync(int id)
        {
            // include marker -- requires return _context.PuntoControls as it's own line with no extra text -- do not delete this comment
            return await _context.RegisterControls
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public RegisterControl GetRegisterControl(int id)
        {
            // include marker -- requires return _context.PuntoControls as it's own line with no extra text -- do not delete this comment
            return _context.RegisterControls
                .FirstOrDefault(p => p.Id == id);
        }

        public async Task AddRegisterControl(RegisterControl registerControl)
        {
            if (registerControl == null)
            {
                throw new ArgumentNullException(nameof(registerControl));
            }

            await _context.RegisterControls.AddAsync(registerControl);
        }

        public void DeleteRegisterControl(RegisterControl registerControl)
        {
            if (registerControl == null)
            {
                throw new ArgumentNullException(nameof(registerControl));
            }

            _context.RegisterControls.Remove(registerControl);
        }

        public void UpdateRegisterControl(RegisterControl registerControl)
        {
            // no implementation for now
            _context.Entry(registerControl).State = EntityState.Modified;
        }

        public bool Saves()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
