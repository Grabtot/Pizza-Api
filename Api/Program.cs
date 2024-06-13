using PizzaApi.Api;
using PizzaApi.Api.Attributes;
using PizzaApi.Application;
using PizzaApi.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;

services.AddPresentation(configuration);
services.AddApplication();
services.AddInfrastructure(configuration);

services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilterAttribute>();
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

WebApplication app = builder.Build();

app.MigrateDatabase(configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();
