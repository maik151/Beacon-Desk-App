using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace BeaconDesk.Api2.Filters
{

    public class CorrelationIdResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value != null)
            {
                dynamic response = objectResult.Value;

                try
                {
                    var prop = objectResult.Value.GetType().GetProperty("CorrelationId");
                    if (prop != null)
                    {
                        response.CorrelationId = context.HttpContext.TraceIdentifier;
                    }
                }
                catch { /* Ignoramos si el objeto no tiene la propiedad */ }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
