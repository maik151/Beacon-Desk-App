using BeaconDesk.Application.Dto.Errors;
using BeaconDesk.Application.Exceptions;
using BeaconDesk.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeaconDesk.Api2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env  )
        {
            _next = next;
            _logger = logger;
            _env = env;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Valores por defecto (Error 500)
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "Ha ocurrido un error inesperado en el servidor."; // Mensaje para el usuario final
            string detail = exception.Message; // Mensaje técnico para ti (Backend/Dev)
            object errorsData = null;



            //Ocultar detalle de error para el entorno de Produccion
            if (_env.IsDevelopment())
            {
                detail = exception.Message;
            }
            else
            {
                detail = null;
            }



            switch (exception)
            {
                case AuthenticationException authEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "No autorizado"; // Título amigable
                    detail = authEx.Message;   // Ej: "Credenciales inválidas"
                    break;

                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Recurso no encontrado";
                    detail = notFoundEx.Message; // Ej: "El usuario con ID 5 no existe"
                    break;

                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Error de validación";
                    detail = "Uno o más campos no cumplen los requisitos.";
                    errorsData = validationEx.Errors; // Los errores específicos van en Data
                    break;

                default:
                    // CASO CRÍTICO (500)
                    // En Producción, por seguridad, NO deberías mostrar 'exception.Message' en 'Detail'
                    // para no dar pistas a hackers. Pero para desarrollo está bien.
                    message = "Error Crítico del Sistema";
                    detail = exception.Message; // Ej: "Connection refused 127.0.0.1..."
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            // Construimos la respuesta usando el constructor de ERROR que hicimos arriba
            var response = new ApiResponse<object>((int)statusCode, message, detail)
            {
                Data = errorsData, // Si hay errores de validación, van aquí
                CorrelationId = context.TraceIdentifier
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true // Opcional: para que se lea bonito en Postman
            };

            var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}