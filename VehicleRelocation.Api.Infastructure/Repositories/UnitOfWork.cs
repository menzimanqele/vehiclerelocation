using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using LinqToDB.Data;
using VehicleRelocation.Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehicleRelocation.Api.Domain;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using VehicleRelocation.Api.Infastructure.Persistence;
using System.Linq;
using System.Transactions;
using LinqToDB;
using LinqToDB.AspNet.Logging;
using Microsoft.EntityFrameworkCore.Storage;


namespace VehicleRelocation.Api.Infastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private  DataConnectionTransaction _dbTransaction;
    private readonly IServiceProvider _serviceProvider;
    //private readonly TDbContext _dbContext;
    private readonly ConcurrentDictionary<string,DataContext> _connections = new ();
    private readonly ILogger<UnitOfWork> _logger;
    private readonly List<DatabaseConfig>? _dbConfigs;
    
    public UnitOfWork(IServiceProvider serviceProvider, ILogger<UnitOfWork> logger,IConfiguration configuration)
    {
        _dbConfigs = configuration.GetSection("Data").Get<List<DatabaseConfig>>();
        if (_dbConfigs == null) throw new ArgumentNullException(nameof(_dbConfigs));
       // _dbContext       = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

       foreach (var dbConfig in _dbConfigs)
       {
           AddConnection(dbConfig);
       }
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger          = logger ?? throw new ArgumentNullException(nameof(logger));
       // _connections = new ConcurrentDictionary<string, DataContext>();
    }

    public TRepository GetRepository<TRepository>(bool startTransaction) where TRepository : class, IRepository
    {
        if (_dbTransaction == null && startTransaction) BeginTransaction();
        var repo = _serviceProvider.GetRequiredService<TRepository>();
        
        return repo;
    }
    
    public virtual async Task BeginTransactionAsync()
    {
        try
        {
            if (_dbTransaction != null) return; // Transaction already started
            //_dbTransaction = await _dbContext.BeginTransactionAsync();
             _dbTransaction = await _dbTransaction.DataConnection.BeginTransactionAsync();

            
            
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
       // _dbContext?.Dispose();
            
        GC.SuppressFinalize(this);
    }
    
    public virtual async ValueTask DisposeAsync()
    {
        if (_dbTransaction != null) await RollbackTransactionAsync();
       // await _dbContext.DisposeAsync();
            
        GC.SuppressFinalize(this);
    }
        
    private void BeginTransaction()
    {
        try
        {
            if (_dbTransaction != null) return; // Transaction already started
           // _dbTransaction = _dbContext.BeginTransaction();
           _dbTransaction.DataConnection.BeginTransaction();
           
           foreach (var connection in _connections)
           {
              var m=   _connections[connection.Key].BeginTransaction();
               _connections[connection.Key].;
           }
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
            /*foreach (var connection in _connections)
            {
                _connections[connection.Key].Transaction.Rollback();
            }*/
        }
        catch (Exception exception)
        {
            _dbTransaction.Dispose();
            _dbTransaction = null;
                
            _logger.LogError(exception, "Failed to roll back the transaction. Error: '{ErrorMessage}'", exception.Message);
            throw;
        }
    }

    private void AddConnection(DatabaseConfig databaseConfig)
    {
        if(string.IsNullOrEmpty(databaseConfig.Provider))
            throw new InvalidEnumArgumentException($"Invalid provider {databaseConfig.Provider}");
        
        DataContext dataContext;

        switch (databaseConfig.Provider)
        {
            case "sqlServer":
                dataContext = new DataContext(new DataOptions()
                    .UseSqlServer(connectionString: databaseConfig.ConnectionString)
                    //.UseDefaultLogging(_serviceProvider)
                );
                break;
            case "postgres":
                dataContext = new DataContext(new DataOptions()
                    .UsePostgreSQL(connectionString: databaseConfig.ConnectionString)
                    //.UseDefaultLogging(_serviceProvider)
                );
                break;
            default:
                throw new InvalidEnumArgumentException($"Unsupported provider {databaseConfig.Provider}");
            
        }

        if(_connections.ContainsKey(databaseConfig.Name))return;
        _connections.TryAdd(databaseConfig.Name, dataContext);
    }
}