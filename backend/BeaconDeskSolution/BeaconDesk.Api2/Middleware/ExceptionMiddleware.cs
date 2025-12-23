using BeaconDesk.Application.Dto.Errors;
using BeaconDesk.Application.Exceptions;
using BeaconDesk.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeaconDesk.Api2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
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
                _logger.LogError(ex, "Ocurrió un error no manejado: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            // 1. Valores iniciales por defecto (Error 500)
            var statusCode = HttpStatusCode.InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Status = (int)statusCode,
                Title = "Error Interno del Servidor",
                Detail = _env.IsDevelopment() ? exception.Message : "Ocurrió un error inesperado en el servidor."
            };

            // 2. 🛡️ DETECCIÓN BLINDADA (RNF-SEG-08):
            // Si el mensaje contiene "bloqueada" o es AuthenticationException, forzamos el 401
            if (exception is AuthenticationException ||
                exception.GetType().Name == "AuthenticationException" ||
                exception.Message.Contains("bloqueada"))
            {
                statusCode = HttpStatusCode.Unauthorized;
                problemDetails.Status = (int)statusCode;
                problemDetails.Title = "Acceso Denegado";
                problemDetails.Detail = exception.Message; // Aquí viaja el mensaje de los 15 minutos
                problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
            }
            else
            {
                // 3. Switch para otros tipos de excepciones específicas
                switch (exception)
                {
                    case NotFoundException notFoundEx:
                        statusCode = HttpStatusCode.NotFound;
                        problemDetails.Status = (int)statusCode;
                        problemDetails.Title = "Recurso no encontrado";
                        problemDetails.Detail = notFoundEx.Message;
                        break;

                    case ValidationException validationEx:
                        statusCode = HttpStatusCode.BadRequest;
                        problemDetails.Status = (int)statusCode;
                        problemDetails.Title = "Error de validación";
                        problemDetails.Detail = "Uno o más campos tienen errores.";
                        problemDetails.Extensions.Add("errors", validationEx.Errors);
                        break;

                    default:
                        // Si cae aquí, se queda con los valores de Error 500 definidos al inicio
                        problemDetails.Title = "Ocurrió un error inesperado";
                        break;
                }
            }

            // 4. Metadatos finales y envío de respuesta
            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);
            context.Response.StatusCode = (int)statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonResponse = JsonSerializer.Serialize(problemDetails, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}