using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PointOfSales.WebUI.Extensions
{
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
