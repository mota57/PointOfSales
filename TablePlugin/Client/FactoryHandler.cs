using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TablePlugin.Core;

namespace TablePlugin.Client
{
    internal static class FactoryHandler
    {
        public static TablePLuginController BuildTablePluginHanlder(HttpRequest request, 
            HttpResponse response, 
            RouteData routeData)
        {
            var MainCtrl = new MainRequestController(request, response, routeData);
            return  new TablePLuginController(MainCtrl, TablePluginOptions.QueryRepositoryInstance);
        }

    }
}
