using FluentResults;
using GameOfLifeApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameOfLifeApi.Filters;

/// <summary>
/// Filter to handle exceptions globally
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = ApiResultHandler.HandleException(Result.Fail("An unexpected error occurred. Please try again later."));

        context.ExceptionHandled = true;
    }
}
