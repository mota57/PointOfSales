using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure;
using PointOfSales.Core.Service;
using PointOfSales.WebUI.Providers;

namespace PointOfSales.WebUI.Extensions
{
    public static class StartupExtensions
    {
        public static void AddApplicationFeatures(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(POSMapperConfiguration));
            // Simple example with dependency injection for a data provider.
            services.AddSingleton<IWeatherProvider, WeatherProviderFake>();
            services.AddDbContext<POSContext>(cfg => cfg.UseSqlite(GlobalVariables.Connection));
            services.AddScoped<POSService>();


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

        }

        
        public static void UseApplicationFeatures(this IApplicationBuilder app)
        {
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

        }
    }
}