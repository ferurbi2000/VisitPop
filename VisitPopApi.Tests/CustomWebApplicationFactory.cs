using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System;
using System.Net.Http;
using VisitPop.Infrastructure.Persistence.Contexts;
using VisitPop.WebApi;

namespace VisitPopApi.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        // checkpoint for respawn to clear the database when spenning up each time
        public static Checkpoint checkpoint = new Checkpoint { };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");            

            builder.ConfigureServices(async services =>
            {
                //services.AddEntityFrameworkInMemoryDatabase();


                // Create a new service provider.
                //var provider = services
                    //.AddEntityFrameworkInMemoryDatabase()
                    //.BuildServiceProvider();

                // Add a database context(VisitPopDbContext) using an in-memory
                // database for testing.
                services.AddDbContext<VisitPopDbContext>(options =>
                {
                    //options.UseInMemoryDatabase("InMemoryDbForTesting");
                    //options.UseInMemoryDatabase("UseInMemoryDatabase");
                    //options.UseInternalServiceProvider(provider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<VisitPopDbContext>();

                    // Ensure the database is created. In this case we all ready created in StartupTesting Class of the Api Project
                    //db.Database.EnsureCreated();                    

                    try
                    {                        
                        await checkpoint.Reset(db.Database.GetDbConnection().ConnectionString);
                    }
                    catch (Exception e)
                    { var mensaje = e.Message; }
                }
            });
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }
    }
}
