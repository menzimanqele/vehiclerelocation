using LinqToDB.Data;
using VehicleRelocation.Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using VehicleRelocation.Api.Infastructure.Persistence;

namespace VehicleRelocation.Api.Infastructure.Repositories;

public class UnitOfWork<TDbContext> : IUnitOfWork
where TDbContext : AppDbContext
{
    private  DataConnectionTransaction _dbTransaction;
    private readonly IServiceProvider _serviceProvider;
    private readonly TDbContext _dbContext;
    private readonly ILogger<UnitOfWork<TDbContext>> _logger;
    
    public UnitOfWork(TDbContext dbContext, IServiceProvider serviceProvider, ILogger<UnitOfWork<TDbContext>> logger)
    {
        _dbContext       = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger          = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TRepository GetRepository<TRepository>(bool startTransaction) where TRepository : class, IRepository
    {
        if (_dbTransaction == null && startTransaction) BeginTransaction();
        return _serviceProvider.GetRequiredService<TRepository>();
    }
    
    public virtual async Task BeginTransactionAsync()
    {
        try
        {
            if (_dbTransaction != null) return; // Transaction already started
            _dbTransaction = await _dbContext.BeginTransactionAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to begin transaction. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_dbTransaction == null) throw new ArgumentNullException(nameof(_dbTransaction), "Database transaction has not been started!");

        try
        {
            await _dbTransaction.CommitAsync();
        }
        catch (Exception exception)
        {
            await _dbTransaction.DisposeAsync();
            _dbTransaction = null;
                
            _logger.LogError(exception, "Failed to commit transaction. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }
    public virtual async Task RollbackTransactionAsync()
    {
        if (_dbTransaction == null) throw new ArgumentNullException(nameof(_dbTransaction), "Database transaction has not been started!");

        try
        {
            await _dbTransaction.RollbackAsync();
        }
        catch (Exception exception)
        {
            await _dbTransaction.DisposeAsync();
            _dbTransaction = null;
                
            _logger.LogError(exception, "Failed to roll back the transaction. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }

    public virtual async Task SaveChangesAsync()
    {
        try
        {
            if (_dbTransaction == null) return;
            await CommitTransactionAsync();
        }
        catch (Exception exception)
        {
            await RollbackTransactionAsync();
            _logger.LogError(exception, "Failed to save the database changes. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }

    public void Dispose()
    {
        if (_dbTransaction != null) RollbackTransaction();
        _dbContext?.Dispose();
            
        GC.SuppressFinalize(this);
    }
    
    public virtual async ValueTask DisposeAsync()
    {
        if (_dbTransaction != null) await RollbackTransactionAsync();
        await _dbContext.DisposeAsync();
            
        GC.SuppressFinalize(this);
    }
        
    private void BeginTransaction()
    {
        try
        {
            if (_dbTransaction != null) return; // Transaction already started
            _dbTransaction = _dbContext.BeginTransaction();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to begin transaction. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }
    
    private void RollbackTransaction()
    {
        if (_dbTransaction == null) throw new ArgumentNullException(nameof(_dbTransaction), "Database transaction has not been started!");

        try
        {
            _dbTransaction.Rollback();
        }
        catch (Exception exception)
        {
            _dbTransaction.Dispose();
            _dbTransaction = null;
                
            _logger.LogError(exception, "Failed to roll back the transaction. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }
}