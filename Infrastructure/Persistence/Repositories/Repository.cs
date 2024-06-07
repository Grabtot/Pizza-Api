using Microsoft.EntityFrameworkCore;
using PizzaApi.Application.Common.Interfaces.Repositories;

namespace PizzaApi.Infrastructure.Persistence.Repositories
{
    public abstract class Repository<TEntity>(PizzaDbContext context) : IRepository<TEntity> where TEntity : class
    {
        private readonly PizzaDbContext _context = context;
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Delate(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<TEntity?> FindAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
