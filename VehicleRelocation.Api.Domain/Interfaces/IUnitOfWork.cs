using System.Data;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;

namespace VehicleRelocation.Api.Domain.Interfaces
{
  public interface IUnitOfWork : IDisposable, IAsyncDisposable
  {
    TRepository GetRepository<TRepository>(bool startTransaction) where TRepository : class, IRepository;
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
  }
}
    