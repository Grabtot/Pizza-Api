using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace PizzaApi.Api.Attributes
{
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (IsNotificationHandlerException(context.Exception))
            {
                LogError(context);
                context.ExceptionHandled = true;

                return;
            }

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

            LogError(context);
        }

        private static bool IsNotificationHandlerException(Exception exception)
        {
            StackTrace stackTrace = new(exception, true);

            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                MethodBase? method = frame.GetMethod();
                if (method != null)
                {
                    Type? declaringType = method.DeclaringType?.DeclaringType;
                    if (declaringType != null && declaringType.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void LogError(ExceptionContext context)
        {
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
