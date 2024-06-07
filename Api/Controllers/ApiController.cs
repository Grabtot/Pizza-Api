using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using System.Diagnostics;

namespace PizzaApi.Api.Controllers
{
    [ApiController]

    public abstract class ApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Problem(Error error)
        {
            LogError(error);

            int statusCode = error.Type switch
            {
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        private static void LogError(Error error)
        {
            StackFrame callerFrame = new StackTrace().GetFrame(2)!;
            var callerMethod = callerFrame?.GetMethod();
            var className = callerMethod?.DeclaringType?.Name;

            Log.Information("Error occurred in method {CallerMethod}({ClassName}): {ErrorType} - {ErrorDescription}",
                callerMethod?.Name,
                className,
                error.Type,
                error.Description);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Problem(List<Error> errors)
        {
            if (errors.Count == 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            return Problem(errors.First());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private IActionResult ValidationProblem(List<Error> errors)
        {
            ModelStateDictionary modelStateDictionary = new();
            foreach (Error error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);
            }
            return ValidationProblem(modelStateDictionary);
        }
    }
}
