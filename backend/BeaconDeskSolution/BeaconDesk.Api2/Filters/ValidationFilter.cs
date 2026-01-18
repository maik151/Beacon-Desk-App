using BeaconDesk.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeaconDesk.Api2.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {

                var errors = context.ModelState.Values
                    .Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToList();


                var message = string.Join(" | ", errors);

                var response = new ApiResponse<string>(400, "Error de validación", message);

                response.CorrelationId = context.HttpContext.TraceIdentifier;
                context.Result = new BadRequestObjectResult(response);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
