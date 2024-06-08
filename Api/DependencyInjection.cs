using Mapster;
using MapsterMapper;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace PizzaApi.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configuration);
            services.AddMapping();

            return services;
        }

        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        private static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            string? logFilePath = configuration.GetSection("Logs:Path").Value;
            bool writeInFile = !bool.TryParse(configuration.GetSection("Logs:WriteInFile").Value,
                out var value) || value;

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("PizzaApi.Infrastructure", LogEventLevel.Debug)
                .MinimumLevel.Information()
                .WriteTo.Console();

            if (!string.IsNullOrEmpty(logFilePath) && writeInFile)
            {
                loggerConfiguration.WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day);
            }

            Log.Logger = loggerConfiguration.CreateLogger();
            services.AddSerilog();

            LogSerilogState(logFilePath, writeInFile);

            return services;
        }

        private static void LogSerilogState(string? logFilePath, bool writeInFile)
        {
            Log.Information("Serilog initialized");

            if (string.IsNullOrEmpty(logFilePath))
            {
                Log.Information("File logging is not configured");
                return;
            }

            if (!writeInFile)
            {
                Log.Information("File logging is disabled");
                return;
            }

            var formattedLogFilePath = FormatLogFilePath(logFilePath);
            Log.Information("Logs are being written to the file: {LogFilePath}", formattedLogFilePath);
        }

        private static string FormatLogFilePath(string logFilePath)
        {
            var slashIndex = logFilePath.LastIndexOf('.');
            if (slashIndex == -1)
            {
                return $"{logFilePath}_{DateTime.Now:yyyyMMdd}";
            }
            return logFilePath.Insert(slashIndex, DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
