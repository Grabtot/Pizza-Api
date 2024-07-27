using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TEntity, TId>(PizzaDbContext context) : IRepository<TEntity, TId> where TEntity : class
    {
        protected readonly PizzaDbContext Context = context;
        protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public virtual void Delate(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual async Task<TEntity?> FindAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
