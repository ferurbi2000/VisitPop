using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Interfaces.Employee;
using VisitPop.Application.Wrappers;
using VisitPop.Domain.Entities;
using VisitPop.Infrastructure.Persistence.Contexts;

namespace VisitPop.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private VisitPopDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public EmployeeRepository(VisitPopDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor
                ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(EmployeeParametersDto employeeParameters)
        {
            if (employeeParameters == null)
            {
                throw new ArgumentNullException(nameof(employeeParameters));
            }

            // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate
            var collection = _context.Employees
                .Include(e => e.EmployeeDepartments)
                as IQueryable<Employee>;

            var sieveModel = new SieveModel
            {
                Sorts = employeeParameters.SortOrder ?? "Id",
                Filters = employeeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Employee>.CreateAsync(collection,
                employeeParameters.PageNumber,
                employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            // include marker -- requires return _context.Empleados as it's own line with no extra text -- do not delete this comment
            return await _context.Employees
                .Include(e => e.EmployeeDepartments)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public Employee GetEmployee(int employeeId)
        {
            // include marker -- requires return _context.Empleados as it's own line with no extra text -- do not delete this comment
            return _context.Employees
                .Include(e => e.EmployeeDepartments)
                .FirstOrDefault(e => e.Id == employeeId);
        }

        public async Task AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await _context.Employees.AddAsync(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _context.Employees.Remove(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            // no implementation for now
            _context.Entry(employee).State = EntityState.Modified;
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
