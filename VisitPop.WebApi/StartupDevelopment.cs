using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VisitPop.Application;
using VisitPop.Infrastructure.Persistence;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.Infrastructure.Persistence.Seeders;
using VisitPop.Infrastructure.Shared;
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
            services.AddSharedInfrastructure(_config);
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
                EmployeeDepartmentSeeder.SeedSampleEmployeeDepartmentData(app.ApplicationServices.GetService<VisitPopDbContext>());
                EmployeeSeeder.SeedSampleEmployeeData(app.ApplicationServices.GetService<VisitPopDbContext>());
                CompanySeeder.SeedEmpresaData(app.ApplicationServices.GetService<VisitPopDbContext>());
                VisitStateSeeder.SeedVisitStateData(app.ApplicationServices.GetService<VisitPopDbContext>());
                OfficeSeeder.SeedSampleOfficeData(app.ApplicationServices.GetService<VisitPopDbContext>());
                ObservacionSeeder.SeedSampleObservacionData(app.ApplicationServices.GetService<VisitPopDbContext>());
                RegisterControlSeeder.SeedRegisterControlData(app.ApplicationServices.GetService<VisitPopDbContext>());
                PersonTypeSeeder.PersonTypeSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());
                VehicleTypeSeeder.VehicleTypeSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());
                VisitTypeSeeder.VisitTypeSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());
                PersonSeeder.PersonSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());

                VisitSeeder.SeedSampleVisitData(app.ApplicationServices.GetService<VisitPopDbContext>());
                VisitPersonSeeder.VisitPersonSampleData(app.ApplicationServices.GetService<VisitPopDbContext>());

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
