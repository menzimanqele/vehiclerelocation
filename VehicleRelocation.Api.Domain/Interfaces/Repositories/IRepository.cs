﻿using System.Linq.Expressions;
using VehicleRelocation.Api.Domain.Entities.Base;
namespace VehicleRelocation.Api.Domain.Interfaces.Repositories
{
    public interface IRepository
    {
        // Used as a marker interface
    }
    
    public interface IRepository<T, TKey, IDbConnection> where T : BaseEntity<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TKey id);
        Task AddSync(T entity);
       // Task<bool> SaveChangesAsync();
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<bool> DeleteAysnc(T entity);
    }
}

