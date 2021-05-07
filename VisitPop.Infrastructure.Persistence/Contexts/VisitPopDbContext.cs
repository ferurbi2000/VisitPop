using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Domain.Entities;

namespace VisitPop.Infrastructure.Persistence.Contexts
{
    public class VisitPopDbContext : DbContext
    {
        public VisitPopDbContext(DbContextOptions<VisitPopDbContext> options)
            : base(options)
        {
        }

        #region DBSet Region - Do Not Delete        
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EstadoVisita> EstadoVisitas { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Observacion> Observaciones { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<TipoPersona> TipoPersonas { get; set; }
        public DbSet<TipoVehiculo> TipoVehiculos { get; set; }
        public DbSet<TipoVisita> TipoVisitas { get; set; }
        public DbSet<PuntoControl> PuntoControles { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<VisitaPersona> VisitaPersonas { get; set; }

        #endregion
    }
}
