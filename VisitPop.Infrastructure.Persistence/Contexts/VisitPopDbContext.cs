using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisitPop.Application.Interfaces;
using VisitPop.Domain.Common;
using VisitPop.Domain.Entities;

namespace VisitPop.Infrastructure.Persistence.Contexts
{
    public class VisitPopDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;

        public VisitPopDbContext(DbContextOptions<VisitPopDbContext> options, IDateTimeService dateTime)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;

        }

        #region DBSet Region - Do Not Delete        
        public DbSet<Company> Companies { get; set; }
        public DbSet<VisitState> VisitStates { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Observacion> Observaciones { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PersonType> PersonTypes { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VisitType> VisitTypes { get; set; }
        public DbSet<RegisterControl> RegisterControls { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<VisitaPersona> VisitaPersonas { get; set; }

        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    //case EntityState.Detached:
                    //    break;
                    //case EntityState.Unchanged:
                    //    break;
                    //case EntityState.Deleted:
                    //    break;
                    case EntityState.Added:
                        entry.Entity.CreatedDate = _dateTime.NowUtc;
                        entry.Entity.IsActive = true;                        
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                                        
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
