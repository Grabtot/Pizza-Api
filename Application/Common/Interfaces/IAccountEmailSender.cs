using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Common.Interfaces
{
    public interface IAccountEmailSender
    {
        Task SendConfirmationEmailAsync(User user, string email, bool isChange = false);
        Task SendPasswordRecoveryEmailAsync(User user, bool sendCode = false);
    }
}