using BeaconDesk.Application.Dto.Errors;
using BeaconDesk.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BeaconDesk.Api2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // El logueo con ILogger está perfecto y es CRÍTICO para el diagnóstico
                _logger.LogError(ex, "Ocurrió un error no manejado: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError; // Por defecto: 500

            // =======================================================
            // 🚨 AJUSTE CRÍTICO: Inicializar aquí el ErrorDetails con la hora
            // para que todos los errores (404, 400, 500) tengan ErrorId y Timestamp.
            // =======================================================
            var errorDetails = new ErrorDetails
            {
                StatusCode = (int)statusCode,
                Message = "Ha ocurrido un error inesperado en el servidor.",
                Timestamp = DateTimeOffset.UtcNow // Establece la hora del evento
                // ErrorId se autogenera en la clase ErrorDetails
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
                    errorDetails.Errors = validationEx.Errors; // Adjuntar los errores
                    break;

                default:
                    // Cualquier otro error (BD, NullReference, etc.) se trata como 500.
                    // El mensaje y el código ya están establecidos por defecto.
                    break;
            }

            // Sincronizar el StatusCode en la respuesta HTTP y en el JSON
            context.Response.StatusCode = (int)statusCode;
            errorDetails.StatusCode = (int)statusCode;

            // Escribe la respuesta JSON en el cuerpo de la respuesta HTTP
            return context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}