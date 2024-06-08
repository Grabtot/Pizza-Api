﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace PizzaApi.Api.Attributes
{
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = context.Exception.Message,
                Instance = context.HttpContext.Request.Path
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;

            Log.Error(context.Exception,
               "Exception was thrown: {ExceptionType} - {Message}, Path: {Path}, Method: {Method}, Headers: {Headers}",
               context.Exception.GetType().Name,
               context.Exception.Message,
               context.HttpContext.Request.Path,
               context.HttpContext.Request.Method,
               context.HttpContext.Request.Headers);
        }

    }
}