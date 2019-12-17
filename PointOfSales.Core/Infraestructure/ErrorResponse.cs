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

    public class OperationResponse
    {
        private ErrorResponse _errorResponse;

        public OperationResponse()
        {

        }

        public OperationResponse(string message)
        {
            Message = message;
        }

        public OperationResponse(Exception ex)
        {
            ErrorResponse = new ErrorResponse(ex);
            Message = ErrorResponse.ToString();
            IsValid = false;
        }

        public bool IsValid { get; set; } = true;

        public string Message { get; set; }

        private ErrorResponse ErrorResponse
        {
            get => _errorResponse;
            set
            {
                _errorResponse = value;
                IsValid = false;
            }
        }
    }
}
