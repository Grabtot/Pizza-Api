namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        void Delate(TEntity entity);
        Task<TEntity?> FindAsync(Guid id);
        void Update(TEntity entity);
    }
}
