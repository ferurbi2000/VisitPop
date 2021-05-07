using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Empleado;
using VisitPop.Application.Interfaces.Empleado;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public EmpleadoRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Empleado>> GetEmpleadosAsync(EmpleadoParametersDto EmpleadoParameters)
        {
            if (EmpleadoParameters == null)
            {
                throw new ArgumentNullException(nameof(EmpleadoParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Empleados
                .Include(e => e.EmployeeDepartments)
                as IQueryable<Empleado>;

            var sieveModel = new SieveModel
            {
                Sorts = EmpleadoParameters.SortOrder ?? "Id",
                Filters = EmpleadoParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Empleado>.CreateAsync(collection,
                EmpleadoParameters.PageNumber,
                EmpleadoParameters.PageSize);
        }

        public async Task<Empleado> GetEmpleadoAsync(int empleadoId)
        {
            // include marker -- requires return _context.Empleados as it's own line with no extra text -- do not delete this comment
            return await _context.Empleados
                .Include(e => e.EmployeeDepartments)
                .FirstOrDefaultAsync(e => e.Id == empleadoId);
        }

        public Empleado GetEmpleado(int empleadoId)
        {
            // include marker -- requires return _context.Empleados as it's own line with no extra text -- do not delete this comment
            return _context.Empleados
                .Include(e => e.EmployeeDepartments)
                .FirstOrDefault(e => e.Id == empleadoId);
        }

        public async Task AddEmpleado(Empleado empleado)
        {
            if (empleado == null)
            {
                throw new ArgumentNullException(nameof(empleado));
            }

            await _context.Empleados.AddAsync(empleado);
        }

        public void DeleteEmpleado(Empleado empleado)
        {
            if (empleado == null)
            {
                throw new ArgumentNullException(nameof(empleado));
            }

            _context.Empleados.Remove(empleado);
        }

        public void UpdateEmpleado(Empleado empleado)
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
