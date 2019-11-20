using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSales.WebUI.Extensions
{
    public static class HttpRequestCustomExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers != null && request.Headers["X-Requested-With"] == "";

        }
    }
}
