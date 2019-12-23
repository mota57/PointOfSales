using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using TablePlugin.Core;

namespace TablePlugin.Client
{
    public static class ApplicationBuilderExtension 
    {
        public static bool IsActive = false;

        //public static IApplicationBuilder UseTablePlugin(this IApplicationBuilder app, Action<PluginOptions> action )
        //{
        //    action(new PluginOptions());
        //    return app;
        //});

        public static IApplicationBuilder UseTablePlugin(this IApplicationBuilder app)
        {
            if (IsActive) return app;
            IsActive = true;
            app.UseRouter(router =>
            {
               
                router.MapGet($"/{TablePluginOptions.RoutePath}/index",  async (req, res, routeData) =>
                {
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.IndexPage();
                });

                router.MapGet($"/{TablePluginOptions.RoutePath}/load", async (req, res, routeData) =>
                {
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.Load();
                });
                
                router.MapGet($"/{TablePluginOptions.RoutePath}/loadById", async (req, res, routeData) =>
                {
                
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.LoadRecordById();
                });


                router.MapPost($"/{TablePluginOptions.RoutePath}/upsert",  async (req, res, routeData) =>
                {
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.Upsert();
                });

                router.MapPost($"/{TablePluginOptions.RoutePath}/delete", async (req, res, routeData) =>
                {
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.Delete();
                });


            });
            return app;
        }
    }
}
