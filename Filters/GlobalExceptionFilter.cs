using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameOfLifeApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // e.g., _logger.LogError(context.Exception, "Unhandled exception occurred.");

        var response = new
        {
            Message = "An unexpected error occurred. Please try again later."
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}
