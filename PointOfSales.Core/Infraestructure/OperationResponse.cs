using Newtonsoft.Json;
using System;

namespace PointOfSales.Core.Infraestructure
{
    public class OperationResponse
    {

        [JsonIgnore]
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




        [JsonIgnore]
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
