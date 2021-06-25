using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using VisitPop.Application.Interfaces.Company;
using VisitPop.Application.Interfaces.Employee;
using VisitPop.Application.Interfaces.EmployeeDepartment;
using VisitPop.Application.Interfaces.Observacion;
using VisitPop.Application.Interfaces.Office;
using VisitPop.Application.Interfaces.Persona;
using VisitPop.Application.Interfaces.PersonType;
using VisitPop.Application.Interfaces.RegisterControl;
using VisitPop.Application.Interfaces.VisitType;
using VisitPop.Application.Interfaces.VehicleType;
using VisitPop.Application.Interfaces.Visita;
using VisitPop.Application.Interfaces.VisitaPersona;
using VisitPop.Application.Interfaces.VisitState;
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
            service.AddScoped<IEmployeeRepository, EmployeeRepository>();
            service.AddScoped<ICompanyRepository, CompanyRepository>();
            service.AddScoped<IVisitStateRepository, VisitStateRepository>();
            service.AddScoped<IOfficeRepository, OfficeRepository>();
            service.AddScoped<IObservacionRepository, ObservacionRepository>();
            service.AddScoped<IRegisterControlRepository, RegisterControlRepository>();
            service.AddScoped<IPersonTypeRepository, PersonTypeRepository>();
            service.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
            service.AddScoped<IVisitTypeRepository, VisitTypeRepository>();
            service.AddScoped<IPersonaRepository, PersonaRepository>();

            service.AddScoped<IVisitaRepository, VisitaRepository>();
            service.AddScoped<IVisitaPersonaRepository, VisitaPersonaRepository>();

            #endregion
        }
    }
}
