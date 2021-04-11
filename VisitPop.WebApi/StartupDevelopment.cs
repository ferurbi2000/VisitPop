using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VisitPop.Application;
using VisitPop.Infrastructure.Persistence;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Seeders;
using VisitPop.WebApi.Extensions;

namespace VisitPop.WebApi
{
    public class StartupDevelopment
    {
        public IConfiguration _config { get; }
        public StartupDevelopment(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCordService("MyCorsPolicy");
            services.AddApplicationLayer();
            services.AddPersistenceInfrastructure(_config);
            //TODO: services.AddSharedInfrastructure(_config);
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();

            #region Dynamic Services
            services.AddSwaggerExtension();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            #region Entity Context Region - Do Not Delete
            using (var context = app.ApplicationServices.GetService<VisitPopDbContext>())
            {
                context.Database.EnsureCreated();

                #region VisitPopDbContext Seeder Region - Do Not Delete
                DepartamentoEmpleadoSeeder.SeedSampleDepartamentoEmpleadoData(app.ApplicationServices.GetService<VisitPopDbContext>());
                EmpleadoSeeder.SeedSampleEmpleadoData(app.ApplicationServices.GetService<VisitPopDbContext>());
                EmpresaSeeder.SeedEmpresaData(app.ApplicationServices.GetService<VisitPopDbContext>());
                EstadoVisitaSeeder.SeedEstadoVisitaData(app.ApplicationServices.GetService<VisitPopDbContext>());
                OficinaSeeder.SeedSampleOficinaData(app.ApplicationServices.GetService<VisitPopDbContext>());                
                ObservacionSeeder.SeedSampleObservacionData(app.ApplicationServices.GetService<VisitPopDbContext>());
                PuntoControlSeeder.SeedPuntoControlData(app.ApplicationServices.GetService<VisitPopDbContext>());
                TipoPersonaSeeder.TipoPersonaSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());
                TipoVehiculoSeeder.TipoVehiculoSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());
                TipoVisitaSeeder.TipoVisitaSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());
                PersonaSeeder.PersonaSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());

                VisitaSeeder.SeedSampleVisitaData(app.ApplicationServices.GetService<VisitPopDbContext>());
                VisitaPersonaSeeder.VisitPersonaSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());

                #endregion
            }
            #endregion

            app.UseCors("MyCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });

            #region Dynamic App
            app.UseSwaggerExtension();
            #endregion
        }
    }
}
