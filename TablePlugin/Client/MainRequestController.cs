using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Linq;

namespace TablePlugin.Client
{
    public class MainRequestController 
    {
        public string AppBaseUrl() => $"{Request.Scheme}://{Request.Host}{Request.PathBase}/tableplugin";


        public MainRequestController(HttpRequest request, HttpResponse response, RouteData routeData)
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

        public async Task Ok()
        {
            await Ok(new { Success = true });
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
