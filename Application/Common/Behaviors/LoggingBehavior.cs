using ErrorOr;
using MediatR;
using Serilog;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PizzaApi.Application.Common.Behaviors
{
    public partial class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var splitedRequestName = SplitRequestName(typeof(TRequest).Name);

            Log.Information($"Starting {splitedRequestName}");

            var response = await next();

            string resultState = response.IsError ? "failed" : "completed successful";
            Log.Information($"{splitedRequestName} {resultState}");

            return response;
        }

        private static string SplitRequestName(string requestName)
        {
            string result = CapitalLetters().Replace(requestName, " $1");

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());
        }

        [GeneratedRegex(@"(\B[A-Z])")]
        private static partial Regex CapitalLetters();
    }
}
