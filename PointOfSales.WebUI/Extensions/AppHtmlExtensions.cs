using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Http;
using System.Threading.Tasks;

namespace PointOfSales.Core.Extensions
{
    public static class AppHtmlExtensions
    {
        public static async Task GenerateApiUrl(this IHtmlHelper helper)
        {

            string url = $"{helper.GetApplicationUrl()}/swagger/v1/swagger.json";

            HttpClient client = new HttpClient();

            string content = await client.GetStringAsync(url);

            JObject obj = JObject.Parse(content);
            JToken paths = obj.GetValue("paths");

            
        }

        public static string GetApplicationUrl(this IHtmlHelper helper)
        {
            var request = helper.ViewContext.HttpContext.Request;

            return $"{request.Scheme}://{request.Host}";
        }

    }
}
