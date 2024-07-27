using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TEntity, TId>(PizzaDbContext context) : IRepository<TEntity, TId> where TEntity : class
    {
        private readonly PizzaDbContext _context = context;
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual void Delate(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<TEntity?> FindAsync(TId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
