using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            services.AddDbContext<POSContext>(cfg => cfg.UseSqlite(connectionString: GlobalVariables.Connection));
            services.AddScoped<POSService>();

            services.AddScoped<IJwtProvider, CustomJwtProvider>();


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }


        public static void AddIdentityFeature(this IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<POSContext>()
           .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; //!!!!!!!!!!!!!!!!!!!!!! Turned off
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();


            //adding claims
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

        }

    }
}
