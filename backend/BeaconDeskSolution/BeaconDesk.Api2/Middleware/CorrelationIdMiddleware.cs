using Serilog.Context;

namespace BeaconDesk.Api2.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _correlationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context) { 
            //Obtenemos el ID de corraltion
            string correlationId = context.Request.Headers[_correlationIdHeader].FirstOrDefault()!;

            //Si este id no existe creamos un ID nuevo
            if (string.IsNullOrEmpty(correlationId)) {
                correlationId = Guid.NewGuid().ToString();
            }

            // Devolvemos el ID de correlación en la respuesta
            context.Response.Headers.TryAdd(_correlationIdHeader, correlationId);


            //Agregamos el ID de correlación al contexto de Serilog
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }


        }


    }
}
