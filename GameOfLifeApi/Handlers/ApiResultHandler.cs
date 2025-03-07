﻿using FluentResults;
using GameOfLifeApi.Helpers;
using GameOfLifeApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeApi.Handlers
{
    /// <summary>
    /// Handler for API results
    /// </summary>
    public static class ApiResultHandler
    {
        public static IActionResult HandleResult(Result<bool[][]> result)
        {
            if(result.IsFailed)
            {
                return new BadRequestObjectResult(new ApiResponseDTO { data = null, message = result?.Errors?[0].Message });
            }

            return new OkObjectResult(new ApiResponseDTO() { data = result.Value, asciiData = AsciiConverter.ParseToAscii(result.Value), message = "Successful request" });
        }

        public static IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsFailed)
            {
                return new BadRequestObjectResult(new ApiResponseDTO { data = null, message = result?.Errors?[0].Message });
            }

            return new OkObjectResult(new ApiResponseDTO() { data = result.Value, message = "Successful request" });
        }

        public static IActionResult HandleException(Result result)
        {
            var httpResult = new ObjectResult(new ApiResponseDTO { data = null, message = result?.Errors?[0].Message });

            httpResult.StatusCode = 500;

            return httpResult;
        }
    }

    
}
