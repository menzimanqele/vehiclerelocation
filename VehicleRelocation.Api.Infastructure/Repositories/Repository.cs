﻿using System.Data;
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
                OpenConnection(connection);
                await connection.InsertAsync(entity);
            }
        }

        private void OpenConnection(DbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            { 
                connection.ConnectionString = _connectionString; 
                connection.Open();
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
                OpenConnection(connection);
               return await connection.GetAllAsync<T>();
            }
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            using (var connection = _dbConnection)
            {
                OpenConnection(connection);
                return await connection.GetAsync<T>(id);
            }
        }
        
        public virtual async Task<bool> DeleteAysnc(T entity)
        {
            using (var connection = GetConnection())
            {
                OpenConnection(connection);
                return await  connection.DeleteAsync<T>(entity);
            }
        }
    }
}

