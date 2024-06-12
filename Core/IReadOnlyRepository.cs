using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core;

public interface IReadOnlyRepository
{
    void SetCommandTimeout(TimeSpan timeout);
        
    IEnumerable<TEntity> FromSqlRaw<TEntity>(
        string sql, bool asNoTracking = false, params object[] parameters)
        where TEntity : class, IEntity;
    
    IEnumerable<TEntity> Get<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity,object>> orderBy = null,
        Expression<Func<TEntity,object>> orderByDescending = null,
        List<Expression<Func<TEntity, object>>> includes = null,
        int? skip = null,
        int? take = null,
        bool asNoTracking = true)
        where TEntity : class, IEntity;

    Task<IEnumerable<TEntity>> GetAsync<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity,object>> orderBy = null,
        Expression<Func<TEntity,object>> orderByDescending = null,
        List<Expression<Func<TEntity,object>>> includes = null,
        int? skip = null,
        int? take = null,
        bool asNoTracking = true)
        where TEntity : class, IEntity;

    TEntity GetOne<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        List<Expression<Func<TEntity, object>>> includes = null,
        bool asNoTracking = true)
        where TEntity : class, IEntity;

    Task<TEntity> GetOneAsync<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        List<Expression<Func<TEntity, object>>> includes = null,
        bool asNoTracking = true)
        where TEntity : class, IEntity;

    TEntity GetFirst<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity,object>> orderBy = null,
        Expression<Func<TEntity,object>> orderByDescending = null,
        List<Expression<Func<TEntity, object>>> includes = null,
        bool asNoTracking = true)
        where TEntity : class, IEntity;

    Task<TEntity> GetFirstAsync<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity,object>> orderBy = null,
        Expression<Func<TEntity,object>> orderByDescending = null,
        List<Expression<Func<TEntity, object>>> includes = null,
        bool asNoTracking = true)
        where TEntity : class, IEntity;

    TEntity GetById<TEntity>(object id, bool asNoTracking = true)
        where TEntity : class, IEntity;

    Task<TEntity> GetByIdAsync<TEntity>(object id, bool asNoTracking = true)
        where TEntity : class, IEntity;

    int Count<TEntity>(Expression<Func<TEntity, bool>> filter = null)
        where TEntity : class, IEntity;

    Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
        where TEntity : class, IEntity;

    bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null)
        where TEntity : class, IEntity;

    Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
        where TEntity : class, IEntity;
}