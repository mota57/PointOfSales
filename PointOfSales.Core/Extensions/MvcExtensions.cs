using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PointOfSales.WebUI.Extensions
{
    public static class AppHtmlExtensions
    {
        public static void GenerateApiUrl(this IHtmlHelper helper)
        {
            //HttpClient client = new HttpClient();
            
        }

    
    }

    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToBytes(this IFormFile formFile)
        {
            if (formFile == null) return default(byte[]);

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }

        }

    }
}
