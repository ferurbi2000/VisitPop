using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VisitPop.Application;
using VisitPop.Infrastructure.Persistence;
using VisitPop.WebApi.Extensions;

namespace VisitPop.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IConfiguration configuration)
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
            app.UseCors("MyCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseErrorHandlingMiddleware();           

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}            

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