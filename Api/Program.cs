using PizzaApi.Api;
using PizzaApi.Api.Attributes;
using PizzaApi.Application;
using PizzaApi.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddPresentation(configuration);
services.AddApplication();
services.AddInfrastructure(configuration);

services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilterAttribute>();
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();
