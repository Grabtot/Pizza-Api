using MediatR;
using PizzaApi.Application.Common.Interfaces;
using PizzaApi.Domain.Users;

namespace PizzaApi.Application.Users.Events
{
    public class UserCreatedEventHandler(IAccountEmailSender emailSender) : INotificationHandler<UserCreatedEvent>
    {
        private readonly IAccountEmailSender _emailSender = emailSender;

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _emailSender.SendConfirmationEmailAsync(notification.User, notification.User.Email!);
        }
    }
}
