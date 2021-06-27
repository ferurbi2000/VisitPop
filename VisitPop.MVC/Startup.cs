using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VisitPop.MVC.Services.Company;
using VisitPop.MVC.Services.Employee;
using VisitPop.MVC.Services.EmployeeDepartment;
using VisitPop.MVC.Services.Office;
using VisitPop.MVC.Services.Person;
using VisitPop.MVC.Services.PersonType;
using VisitPop.MVC.Services.RegisterControl;
using VisitPop.MVC.Services.VehicleType;
using VisitPop.MVC.Services.VisitState;
using VisitPop.MVC.Services.VisitType;

namespace VisitPop.MVC
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmployeeDepartmentRepository, EmployeeDepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IVisitStateRepository, VisitStateRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IPersonTypeRepository, PersonTypeRepository>();
            services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
            services.AddScoped<IVisitTypeRepository, VisitTypeRepository>();
            services.AddScoped<IRegisterControlRepository, RegisterControlRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddMvc();

            services.AddControllers();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStatusCodePages();

            app.UseStaticFiles();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
