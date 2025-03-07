using System.Linq.Expressions;
using Core.EFCore.Core;
using Microsoft.EntityFrameworkCore;

namespace Core.EFCore;

public class EfReadOnlyRepository<TContext> : IReadOnlyRepository where TContext : DbContext
{
    protected readonly TContext Context;

    public EfReadOnlyRepository(TContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        
        Context.ChangeTracker.AutoDetectChangesEnabled = false;
        Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    protected virtual IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity,object>>? orderBy = null, Expression<Func<TEntity,object>>? orderByDescending = null, List<Expression<Func<TEntity, object>>>? includes = null, int? skip = null, int? take = null, bool asNoTracking = true) where TEntity : class, IEntity
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (filter != null) query = query.Where(filter);
        if (includes != null) query = includes.Aggregate(query, (current, expression) => current.Include(expression));
        if (orderBy != null) query = query.OrderBy(orderBy);
        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);
        if (asNoTracking) query = query.AsNoTracking();

        return query;
    }
    
    public virtual IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, Expression<Func<TEntity, object>> orderByDescending = null, List<Expression<Func<TEntity, object>>> includes = null, int? skip = null, int? take = null, bool asNoTracking = true)
        where TEntity : class, IEntity => GetQueryable(filter, orderBy, orderByDescending, includes, skip, take, asNoTracking).ToList();
    
    public virtual async Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, Expression<Func<TEntity, object>> orderByDescending = null, List<Expression<Func<TEntity, object>>> includes = null, int? skip = null, int? take = null, bool asNoTracking = true)
        where TEntity : class, IEntity => await GetQueryable(filter, orderBy, orderByDescending,includes, skip, take, asNoTracking).ToListAsync();

    public virtual TEntity GetOne<TEntity>(Expression<Func<TEntity, bool>> filter = null, List<Expression<Func<TEntity, object>>> includes = null, bool asNoTracking = true)
        where TEntity : class, IEntity => GetQueryable(filter, null, null, includes, asNoTracking: asNoTracking).SingleOrDefault();

    public virtual async Task<TEntity> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, List<Expression<Func<TEntity, object>>> includes = null, bool asNoTracking = true)
        where TEntity : class, IEntity => await GetQueryable(filter, null, null, includes, asNoTracking: asNoTracking).SingleOrDefaultAsync();

    public virtual TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, Expression<Func<TEntity,object>> orderByDescending = null, List<Expression<Func<TEntity, object>>> includes = null, bool asNoTracking = true)
        where TEntity : class, IEntity => GetQueryable(filter, orderBy, orderByDescending, includes, asNoTracking: asNoTracking).FirstOrDefault();
    
    public virtual async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, Expression<Func<TEntity,object>> orderByDescending = null, List<Expression<Func<TEntity, object>>> includes = null, bool asNoTracking = true)
        where TEntity : class, IEntity => await GetQueryable<TEntity>(filter, orderBy, orderByDescending, includes, asNoTracking: asNoTracking).FirstOrDefaultAsync();

    public virtual TEntity GetById<TEntity>(object id, bool asNoTracking = true)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        if (asNoTracking) dbSet.AsNoTracking();
        var entity = dbSet.Find(id);
        
        return entity;
    }

    public virtual async Task<TEntity> GetByIdAsync<TEntity>(object id, bool asNoTracking = true)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        if (asNoTracking) dbSet.AsNoTracking();
        var entity = await dbSet.FindAsync(id);
        
        return entity;
    }

    public virtual int Count<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntity => GetQueryable(filter).Count();

    public virtual Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntity => GetQueryable(filter).CountAsync();

    public virtual bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntity => GetQueryable<TEntity>(filter).Any();

    public virtual Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntity => GetQueryable<TEntity>(filter).AnyAsync();
    
    public void SetCommandTimeout(TimeSpan timeout) => Context.Database.SetCommandTimeout(timeout);

    public IEnumerable<TEntity> FromSqlRaw<TEntity>(string sql, bool asNoTracking = true, params object[] parameters)
        where TEntity : class, IEntity
    {
        var query = Context.Set<TEntity>().FromSqlRaw(sql);
        if (asNoTracking) query = query.AsNoTracking();

        return query;
    }
}