namespace PizzaApi.Domain.Common.Interfaces
{
    public interface IAuditableEntity
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}