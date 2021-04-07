using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Interfaces;
using VisitPop.Application.Interfaces.DepartamentoEmpleado;
using VisitPop.Application.Interfaces.Empleado;
using VisitPop.Application.Interfaces.Empresa;
using VisitPop.Application.Interfaces.EstadoVisita;
using VisitPop.Application.Interfaces.Observacion;
using VisitPop.Application.Interfaces.Oficina;
using VisitPop.Application.Interfaces.PuntoControl;
using VisitPop.Application.Interfaces.VisitPop;
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

            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IPersonRepository, PersonRepository>();
            service.AddScoped<IDepartamentoEmpleadoRepository, DepartamentoEmpleadoRepository>();
            service.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            service.AddScoped<IEmpresaRepository, EmpresaRepository>();
            service.AddScoped<IEstadoVisitaRepository, EstadoVisitaRepository>();
            service.AddScoped<IOficinaRepository, OficinaRepository>();
            service.AddScoped<IObservacionRepository, ObservacionRepository>();
            service.AddScoped<IPuntoControlRepository, PuntoControlRepository>();

            #endregion
        }
    }
}
