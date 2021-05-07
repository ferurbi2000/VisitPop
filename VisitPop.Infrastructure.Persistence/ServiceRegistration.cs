using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using VisitPop.Application.Interfaces.EmployeeDepartment;
using VisitPop.Application.Interfaces.Empleado;
using VisitPop.Application.Interfaces.Empresa;
using VisitPop.Application.Interfaces.EstadoVisita;
using VisitPop.Application.Interfaces.Observacion;
using VisitPop.Application.Interfaces.Oficina;
using VisitPop.Application.Interfaces.Persona;
using VisitPop.Application.Interfaces.PuntoControl;
using VisitPop.Application.Interfaces.TipoPersona;
using VisitPop.Application.Interfaces.TipoVehiculo;
using VisitPop.Application.Interfaces.TipoVisita;
using VisitPop.Application.Interfaces.Visita;
using VisitPop.Application.Interfaces.VisitaPersona;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Repositories;

namespace VisitPop.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            #region DBContext --Do Not Delete            
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<VisitPopDbContext>(options =>
                    options.UseInMemoryDatabase($"VisitPopDb"));
            }
            else
            {
                service.AddDbContext<VisitPopDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("VisitPopDb"),
                        builder => builder.MigrationsAssembly(typeof(VisitPopDbContext).Assembly.FullName)));
            }
            #endregion

            service.AddScoped<SieveProcessor>();

            #region Repositories -- Do Not Delete

            service.AddScoped<IEmployeeDepartmentRepository, EmployeeDepartmentRepository>();
            service.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            service.AddScoped<IEmpresaRepository, EmpresaRepository>();
            service.AddScoped<IEstadoVisitaRepository, EstadoVisitaRepository>();
            service.AddScoped<IOficinaRepository, OficinaRepository>();
            service.AddScoped<IObservacionRepository, ObservacionRepository>();
            service.AddScoped<IPuntoControlRepository, PuntoControlRepository>();
            service.AddScoped<ITipoPersonaRepository, TipoPersonaRepository>();
            service.AddScoped<ITipoVehiculoRepository, TipoVehiculoRepository>();
            service.AddScoped<ITipoVisitaRepository, TipoVisitaRepository>();
            service.AddScoped<IPersonaRepository, PersonaRepository>();

            service.AddScoped<IVisitaRepository, VisitaRepository>();
            service.AddScoped<IVisitaPersonaRepository, VisitaPersonaRepository>();

            #endregion
        }
    }
}
