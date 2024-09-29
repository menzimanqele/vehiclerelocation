using System.Data;
using System.Linq.Expressions;
using VehicleRelocation.Api.Domain.Entities.Base;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using System.Data.Common;
using Dapper.Contrib.Extensions;
using VehicleRelocation.Api.Domain;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace VehicleRelocation.Api.Infastructure.Repositories
{
    public abstract class Repository <T,TKey, IDbConnection> : IRepository<T,TKey, IDbConnection> where T : BaseEntity<TKey>
	{
        public readonly DatabaseConfig _databaseConfig;
        public readonly DbConnection _dbConnection;
        private readonly string _connectionString;
        public Repository(IOptions<DatabaseConfig> databaseConfigOptions)
		{
            _databaseConfig = databaseConfigOptions.Value;
            _connectionString = _databaseConfig.ConnectionString;
            _dbConnection = GetConnection();
		}

        public  DbConnection GetConnection()
        {
            switch (_databaseConfig.Provider)
            {
                case "postgres":
                    return new Npgsql.NpgsqlConnection(_databaseConfig.ConnectionString);
                case "sqlServer":
                    return new SqlConnection(_connectionString);
                default:
                    throw new ArgumentException("Invalid connection type");
            }

        }

        public virtual async Task AddSync(T entity)
        {
            using (var connection = _dbConnection)
            {
                if (connection.State == ConnectionState.Closed)
                { 
                    connection.ConnectionString = _connectionString;
                    await connection.OpenAsync();
                }

              //  using var dbTransaction = await connection.BeginTransactionAsync(); 
                await connection.InsertAsync(entity);
              //  await dbTransaction.CommitAsync();
        
            }
        }

        public Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
               return await connection.GetAllAsync<T>();
            }
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                return await connection.GetAsync<T>(id);
            }
        }

        // public async Task<bool> SaveChangesAsync()
        // {
        //     if (_dbConnection.State == ConnectionState.Open)
        //     {
        //         using var dbTransaction = _dbConnection.BeginTransaction();
        //         await dbTransaction.CommitAsync();
        //     }
        //     throw new InvalidOperationException("Connection lost");
        // }
        //
        //

        public virtual async Task<bool> DeleteAysnc(T entity)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                return await  connection.DeleteAsync<T>(entity);
            }
        }
    }
}

