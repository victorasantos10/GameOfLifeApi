using FluentResults;
using GameOfLifeApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeApi.Handlers
{
    public static class ApiResultHandler
    {
        public static IActionResult HandleResult<T>(Result<T> result)
        {
            if(result.IsFailed)
            {
                return new BadRequestObjectResult(new ApiResponseDTO { data = null, message = result?.Errors?[0].Message });
            }

            return new OkObjectResult(new ApiResponseDTO() { data = result.Value, message = "Successful request" });
        }

        public static IActionResult HandleException<T>(Result<T> result)
        {
            var httpResult = new ObjectResult(new ApiResponseDTO { data = null, message = result?.Errors?[0].Message });

            httpResult.StatusCode = 500;

            return httpResult;
        }
    }

    
}
