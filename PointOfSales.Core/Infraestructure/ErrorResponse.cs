using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.Core.Infraestructure
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {

        }

        public ErrorResponse(Exception ex)
        {
            Error = $"Message: {ex.Message}\n Stack Trace: {ex.StackTrace}";
        }

        public string FriendlyMessage
            => "Sorry an error occurred, please contant the developer";
        public string Error { get; set; }
    }
}
