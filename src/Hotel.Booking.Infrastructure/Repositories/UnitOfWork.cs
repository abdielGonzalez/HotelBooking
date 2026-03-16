using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hotel.Booking.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly CardsBdContext _context;

        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(CardsBdContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _context.SaveChangesAsync(ct);
        }
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                return; // ya hay una transacción activa
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No hay una transacción activa.");

            await _context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);

            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
