using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PointOfSales.Core.Entities;
using PointOfSales.Core.Infraestructure;
using PointOfSales.Core.Service;
using PointOfSales.WebUI.Providers;
using System;

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


        public static void AddIdentitySection(this IServiceCollection services)
        {


            services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<POSContext>()
           .AddDefaultTokenProviders();


            // services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AddPermissionsToUserClaims>();

           
            services.AddSingleton<IEmailSender, EmailSender>();


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                //options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
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
