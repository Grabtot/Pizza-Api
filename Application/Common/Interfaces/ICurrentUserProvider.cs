namespace PizzaApi.Application.Common.Interfaces
{
    public interface ICurrentUserProvider
    {
        Guid? UserId { get; }
        string? UserName { get; }
    }
}
