using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace TablePlugin
{
    
    public static class Constants
    {
        public static string DatabaseName { get; set; } = "MyData.db";
    }

    public static class Extensions 
    {
        public static bool IsActive = false;

        public static IApplicationBuilder UseTablePlugin(this IApplicationBuilder app)
        {
            if (IsActive) return app;
            IsActive = true;

            app.UseRouter(router =>
            {
                router.MapGet("/tableplugin/index",  async (req, res, routeData) =>
                {
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.IndexPage();
                });

                router.MapPost("/tableplugin/create",  async (req, res, routeData) =>
                {
                    TablePLuginHandler tablHandler = FactoryHandler.BuildTablePluginHanlder(req, res, routeData);
                    await tablHandler.CreateRecord();
                });


            });
            return app;
        }
    }

    public static class FactoryHandler
    {
        public static TablePLuginHandler BuildTablePluginHanlder(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            var mainRequestHandler = new MainRequestHandler(request, response, routeData);
            return  new TablePLuginHandler(mainRequestHandler);
        }

    }

    public class RequestOptions
    {
        public Stream IndexStream() {
            return typeof(Extensions)
            .GetTypeInfo()
            .Assembly
            .GetManifestResourceStream("TablePlugin.index.html");
       }
    }

    public class  TablePLuginHandler
    {

        protected RequestOptions _options = new RequestOptions();
        MainRequestHandler Handler;

        public TablePLuginHandler(MainRequestHandler handler)
        {
            Handler = handler;
        }

        public async Task IndexPage()
        {
            using (var stream = _options.IndexStream())
            {
                var htmlBuilder = new StringBuilder(new StreamReader(stream).ReadToEnd());
                foreach (var entry in GetIndexArguments())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }
                await Handler.Ok(htmlBuilder.ToString());
            }
        }

        public async Task Load()
        {

            using (var db = new LiteDatabase(Constants.DatabaseName))
            {
                var records = db.GetCollection<QueryRecord>("queryrecords");
                await Handler.Ok(records.FindAll());
            }
        }

        public async Task CreateRecord()
        {
            var record = Handler.ParseBody<QueryRecord>();
            var mapper = BsonMapper.Global;
            mapper.Entity<QueryRecord>()
            .DbRef(x => x.QueryFields, "queryfields");

            using (var db = new LiteDatabase(Constants.DatabaseName))
            {
                var qrecords = db.GetCollection<QueryRecord>("queryrecords");
                qrecords.Insert(record);
            }

            await Handler.Created(record);
        }

        private IDictionary<string, string> GetIndexArguments()
        {
            return new Dictionary<string, string>()
            {
                { "%(BaseUrl)", Handler.AppBaseUrl() },
            };
        }

    }

   

    public class MainRequestHandler 
    {
        public string AppBaseUrl() => $"{Request.Scheme}://{Request.Host}{Request.PathBase}/tableplugin";


        public MainRequestHandler(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            Request = request;
            Response = response;
            RouteData = routeData;
        }

        public T ParseBody<T>()
        {
            var body = new StreamReader(Request.Body).ReadToEnd();
            var record =   JsonConvert.DeserializeObject<T>(body);
            return record;
        }

        public async Task Created(object content)
        {
            var result = JsonConvert.SerializeObject(content);
            Response.ContentType = "application/json";
            Response.StatusCode = 201;
            await Response.WriteAsync(result, Encoding.UTF8);
        }
        public async Task Ok(object contentObj)
        {
            await Ok(JsonConvert.SerializeObject(contentObj));
        }
        public async Task Ok(string content )
        {
            Response.StatusCode = 200;
            await this.Response.WriteAsync(content);
        }

        public HttpRequest Request { get; }
        public HttpResponse Response { get; }
        public RouteData RouteData { get; }
    }
}
