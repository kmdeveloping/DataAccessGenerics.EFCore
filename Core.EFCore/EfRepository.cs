using Core.EFCore.Core;
using Microsoft.EntityFrameworkCore;

namespace Core.EFCore;

public class EfRepository<TContext> : EfReadOnlyRepository<TContext>, IRepository where TContext : DbContext
{
    public EfRepository(TContext context) : base(context)
    {
    }
    
        public virtual TEntity Create<TEntity>(TEntity entity, string createdBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        if (asNoTracking)
            dbSet.AsNoTracking();
        
        if (entity is IModifiableEntity modifiableEntity)
        {
            if (modifiableEntity.CreatedDt == default)
                modifiableEntity.CreatedDt = DateTime.UtcNow;
        }
        
        return dbSet.Add(entity).Entity;
    }

    public virtual void CreateRange<TEntity>(IEnumerable<TEntity> entities, string createdBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        if (asNoTracking)
            dbSet.AsNoTracking();
        
        var entityList = entities.ToList();
        foreach (var entity in entityList)
        {
            if (entity is IModifiableEntity modifiableEntity)
            {
                if(modifiableEntity.CreatedDt == default)
                    modifiableEntity.CreatedDt = DateTime.UtcNow;
            }
        }
        dbSet.AddRange(entityList);
    }
        
    public virtual void Update<TEntity>(TEntity entity, string modifiedBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        if (asNoTracking)
            dbSet.AsNoTracking();
        
        if (entity is IModifiableEntity)
            ((IModifiableEntity)entity).ModifiedDt = DateTime.UtcNow;

        var entityEntry = dbSet.Entry(entity);
        if (entityEntry.State == EntityState.Detached)
            dbSet.Attach(entity);
        
        entityEntry.State = EntityState.Modified;
    }
    
    public virtual async Task UpdateAndSaveAsync<TEntity>(TEntity entity, string modifiedBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        if (asNoTracking)
            dbSet.AsNoTracking();
        
        if (entity is IModifiableEntity)
            ((IModifiableEntity)entity).ModifiedDt = DateTime.UtcNow;

        var entityEntry = dbSet.Entry(entity);
        if (entityEntry.State == EntityState.Detached)
            dbSet.Attach(entity);
        
        entityEntry.State = EntityState.Modified;
        
        await SaveAsync();
    }

    public virtual void Delete<TEntity>(object id)
        where TEntity : class, IEntity
    {
        var dbSet = Context.Set<TEntity>();
        TEntity entity = dbSet.Find(id);
        Delete(entity);
    }

    public virtual void Delete<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        // TODO: implement soft-delete?

        var dbSet = Context.Set<TEntity>();
        if (dbSet.Entry(entity).State == EntityState.Detached)
        {
            dbSet.Attach(entity);
        }
        dbSet.Remove(entity);
    }

    public virtual void Save()
    {
        try
        {
            Context.SaveChanges();
            Context.ChangeTracker.Clear();
        }
        catch(EntityValidationException e)
        {
            ThrowEnhancedValidationException(e);
        }
    }

    public virtual async Task SaveAsync()
    {
        try
        {
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();
        }
        catch(EntityValidationException e)
        {
            ThrowEnhancedValidationException(e);
        }
    }

    public virtual int ExecuteSqlInterpolated(System.FormattableString sqlString)
    {
        return Context.Database.ExecuteSqlInterpolated(sqlString);
    }

    public virtual async Task<int> ExecuteSqlInterpolatedAsync(System.FormattableString sqlString, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await Context.Database.ExecuteSqlInterpolatedAsync(sqlString, cancellationToken);
    }

    public virtual int ExecuteSqlRaw(string sql, IEnumerable<object> parameters)
    {
        return Context.Database.ExecuteSqlRaw(sql, parameters);
    }

    public virtual async Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await Context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }

    protected virtual void ThrowEnhancedValidationException(EntityValidationException e)
    {
        var errorMessages = e.EntityValidationErrors.SelectMany(x => x.ErrorMessage);

        var fullErrorMessage = string.Join("; ", errorMessages);
        var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
        throw new EntityValidationException(exceptionMessage, e.EntityValidationErrors);
    }
}