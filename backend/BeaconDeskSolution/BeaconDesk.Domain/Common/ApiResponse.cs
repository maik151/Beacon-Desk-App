using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeaconDesk.Domain.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // No se envía si es null (para limpiar respuestas 200)
        public string Detail { get; set; }
        public string CorrelationId { get; set; }
        public T Data { get; set; }



        public ApiResponse(T data, string message = null, int statusCode = 200)
        {
            StatusCode = statusCode;
            Success = true;
            Message = message ?? "Operación exitosa";
            Data = data;
            Detail = null;
        }

        public ApiResponse(int statusCode, string message, string detail = null)
        {
            StatusCode = statusCode;
            Success = false;
            Message = message;
            Detail = detail; 
        }


    }
}
