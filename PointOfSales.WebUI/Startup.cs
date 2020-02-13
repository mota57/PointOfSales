using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PointOfSales.Core.Entities;
using PointOfSales.WebUI.Extensions;
using Microsoft.AspNetCore.Routing;
using TablePlugin.Client;
using TablePlugin.Core;
using Microsoft.Extensions.Logging;

namespace PointOfSales.WebUI
{
    public class Startup
    {
        public ILogger<Startup> Logger { get; set; }
        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment env)
        {
            this.Logger = logger;
            Configuration = configuration;
            env.EnvironmentName = "Development";

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configSection = Configuration.GetSection("DatabaseConfig");
            GlobalVariables.Connection = configSection[configSection["DBKEY"]];
            this.Logger.LogDebug(GlobalVariables.Connection);

            services.AddIdentityFeature();

            // Add framework services.
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                })
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddApplicationFeatures();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error");

                // Webpack initialization with hot-reload.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
          

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //TODO this
            //app.UseTablePlugin((options) => {
            //    options.SQLConnectionName = GlobalVariables.Connection;
            //    options.DatabaseProvider = TablePlugin.Core.DatabaseProvider.SQLite;
            //});
            app.UseTablePlugin();
            TablePluginOptions.SQLConnectionName = GlobalVariables.Connection;
            TablePluginOptions.DatabaseProvider = TablePlugin.Core.DatabaseProvider.SQLite;

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });



        }
    }


}
