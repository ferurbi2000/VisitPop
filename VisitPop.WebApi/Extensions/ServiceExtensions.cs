using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitPop.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        #region Swagger Region - Do Not Delete
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "VisitPop API Service",
                        Description = "API for mapping VisitPop.",
                        Contact = new OpenApiContact
                        {
                            Name = "Fernando Urbina",
                            Email = "ferurbi2000@hotmail.com"
                            //Url= new Uri("https")
                        },
                    });
            });
        }
        #endregion

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // use default version when version is not specified
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API version supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }

        public static void AddCordService(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
            });
        }
    }
}
