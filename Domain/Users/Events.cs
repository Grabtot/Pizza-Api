using MediatR;

namespace PizzaApi.Domain.Users
{
    public record UserCreatedEvent(User User) : INotification;
}
