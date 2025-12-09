using BeaconDesk.Application.Dto.Errors;
using BeaconDesk.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Importante para el ILogger
using System;
using System.Net; // Importante para HttpStatusCode
using System.Threading.Tasks;

namespace BeaconDesk.Api2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger; // Variable para el logger

        // Constructor: Recibe RequestDelegate y el ILogger
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger; // CRÍTICO: Asignar el logger inyectado
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // CRÍTICO: Loguear el error completo con el stack trace para logs internos
                _logger.LogError(ex, "Ocurrió un error no manejado: {Message}", ex.Message);

                // Formatear y enviar la respuesta limpia al cliente
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError; // Por defecto: 500

            // Inicializa el modelo de error con valores predeterminados (para el error 500)
            var errorDetails = new ErrorDetails
            {
                StatusCode = (int)statusCode,
                Message = "Ha ocurrido un error inesperado en el servidor."
            };

            // Mapeo de Excepciones Personalizadas a Códigos HTTP
            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound; // 404
                    errorDetails.Message = notFoundEx.Message;
                    break;

                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    errorDetails.Message = validationEx.Message;
                    // CRÍTICO: Adjuntar los errores de validación
                    errorDetails.Errors = validationEx.Errors;
                    break;

                default:
                    // Cualquier otro error (BD, NullReference, etc.) se trata como 500.
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            errorDetails.StatusCode = (int)statusCode; // Sincroniza el código del objeto JSON

            // Escribe la respuesta JSON en el cuerpo de la respuesta HTTP
            return context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}