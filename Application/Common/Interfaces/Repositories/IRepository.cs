namespace PizzaApi.Application.Common.Interfaces.Repositories
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        void Delate(TEntity entity);
        Task<TEntity?> FindAsync(TId id);
        void Update(TEntity entity);
    }
}
