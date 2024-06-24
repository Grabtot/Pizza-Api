using Microsoft.EntityFrameworkCore.Storage;
using PizzaApi.Application.Common.Interfaces;

namespace PizzaApi.Infrastructure.Persistence
{
    public class UnitOfWork(PizzaDbContext dbContext) : IUnitOfWork
    {
        private readonly PizzaDbContext _dbContext = dbContext;
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = _dbContext.Database.CurrentTransaction
                ?? await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _transaction.RollbackAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
