using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TablePlugin.Core;

namespace TablePlugin.Client
{
    public static class FactoryHandler
    {
        public static TablePLuginHandler BuildTablePluginHanlder(HttpRequest request, 
            HttpResponse response, 
            RouteData routeData)
        {
            var mainRequestHandler = new MainRequestHandler(request, response, routeData);
            return  new TablePLuginHandler(mainRequestHandler, TablePluginOptions.QueryRepositoryInstance);
        }

    }
}
