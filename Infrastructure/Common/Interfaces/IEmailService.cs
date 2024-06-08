using PizzaApi.Infrastructure.Services;

namespace PizzaApi.Infrastructure.Common.Interfaces
{
    public interface IEmailService
    {
        Task Send(string toAddress, string subject, string? body = "");
    }
}