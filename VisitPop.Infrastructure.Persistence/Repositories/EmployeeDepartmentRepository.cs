using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Interfaces.EmployeeDepartment;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class EmployeeDepartmentRepository : IEmployeeDepartmentRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public EmployeeDepartmentRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<EmployeeDepartment>> GetEmployeeDepartmentsAsync(EmployeeDepartmentParametersDto employeeDepartmentParameters)
        {
            if (employeeDepartmentParameters == null)
            {
                throw new ArgumentNullException(nameof(employeeDepartmentParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.EmployeeDepartments
                as IQueryable<EmployeeDepartment>;

            var sieveModel = new SieveModel
            {
                Sorts = employeeDepartmentParameters.SortOrder ?? "Id",
                Filters = employeeDepartmentParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<EmployeeDepartment>.CreateAsync(collection,
                employeeDepartmentParameters.PageNumber,
                employeeDepartmentParameters.PageSize);
        }

        public async Task<EmployeeDepartment> GetEmployeeDepartmentAsync(int employeeDepartmentId)
        {
            // include marker -- requires return _context.DepartamentoEmpleados as it's own line with no extra text -- do not delete this comment
            return await _context.EmployeeDepartments
                .FirstOrDefaultAsync(d => d.Id == employeeDepartmentId);
        }

        public EmployeeDepartment GetEmployeeDepartment(int employeeDepartmentId)
        {
            // include marker -- requires return _context.DepartamentoEmpleados as it's own line with no extra text -- do not delete this comment
            return _context.EmployeeDepartments
                .FirstOrDefault(d => d.Id == employeeDepartmentId);
        }

        public async Task AddEmployeeDepartment(EmployeeDepartment employeeDepartment)
        {
            if (employeeDepartment == null)
            {
                throw new ArgumentNullException(nameof(employeeDepartment));
            }

            await _context.EmployeeDepartments.AddAsync(employeeDepartment);
        }

        public void DeleteEmployeeDepartment(EmployeeDepartment employeeDepartment)
        {
            if (employeeDepartment == null)
            {
                throw new ArgumentNullException(nameof(employeeDepartment));
            }

            _context.EmployeeDepartments.Remove(employeeDepartment);

        }

        public void UpdateEmployeeDepartment(EmployeeDepartment employeeDepartment)
        {
            // no implementation for now
            _context.Entry(employeeDepartment).State = EntityState.Modified;
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
