using BeaconDesk.Application.Dto.Errors;
using BeaconDesk.Application.Exceptions;
using BeaconDesk.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                _logger.LogError(ex, "Ocurrió un error no manejado: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var statusCode = HttpStatusCode.InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path, 
                Status = (int)statusCode,        
                Title = "Ocurrió un error inesperado", 
                Detail = _env.IsDevelopment() ? exception.Message : "Consulte los logs para más detalles."
            };


            switch (exception)
            {
                case AuthenticationException authEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    problemDetails.Status = (int)statusCode;
                    problemDetails.Title = "No autorizado";
                    problemDetails.Detail = authEx.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                    break;

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
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Title = "Error Interno del Servidor";
                    // En Prod ocultamos el detalle
                    if (!_env.IsDevelopment()) problemDetails.Detail = null;
                    break;
            }

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