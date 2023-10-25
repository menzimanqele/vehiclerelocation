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
        public Repository(IOptions<DatabaseConfig> databaseConfigOptions)
		{
            _databaseConfig = databaseConfigOptions.Value;
            _dbConnection = GetConnection();
		}

        public  DbConnection GetConnection()
        {
            switch (_databaseConfig.ConnectionType)
            {
                case "postgress":
                    return new Npgsql.NpgsqlConnection(_databaseConfig.ConnnectionString);
                case "sqlServer":
                    return new SqlConnection(_databaseConfig.ConnnectionString);
                default:
                    throw new ArgumentException("Invalid connection type");
            }

        }

        public virtual async void AddSync(T entity)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                await connection.InsertAsync(entity);
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

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> DeleteAysnc(T entity)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                return await  connection.DeleteAsync<T>(entity);
            }
        }
    }
}

