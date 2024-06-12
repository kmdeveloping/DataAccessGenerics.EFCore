namespace Core;

public interface IRepository : IReadOnlyRepository
{
        /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="createdBy"></param>
    /// <param name="asNoTracking"></param>
    /// <returns></returns>
    TEntity Create<TEntity>(TEntity entity, string createdBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity;

    /// <summary>
    /// Creates a range of entities.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entities"></param>
    /// <param name="createdBy"></param>
    void CreateRange<TEntity>(IEnumerable<TEntity> entities, string createdBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity;

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="modifiedBy"></param>
    /// <param name="asNoTracking"></param>
    void Update<TEntity>(TEntity entity, string modifiedBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity;
    
    /// <summary>
    /// Updates the specified entity and then saves context.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="modifiedBy"></param>
    /// <param name="asNoTracking"></param>
    Task UpdateAndSaveAsync<TEntity>(TEntity entity, string modifiedBy = null, bool asNoTracking = false)
        where TEntity : class, IEntity;

    /// <summary>
    /// Deletes the entity with the specified id from the database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="id"></param>
    void Delete<TEntity>(object id)
        where TEntity : class, IEntity;

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    void Delete<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    /// <summary>
    /// Saves any pending changes to the underlying database.
    /// </summary>
    void Save();

    /// <summary>
    /// Saves any pending changes to the underlying database.
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    ///     <para>
    ///         Executes the given SQL against the database and returns the number of rows affected.
    ///     </para>
    ///     <para>
    ///         Note that this method does not start a transaction. To use this method with
    ///         a transaction, first call <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.BeginTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.IsolationLevel)" /> or <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.UseTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.Common.DbTransaction)" />.
    ///     </para>
    ///     <para>
    ///         Note that the current <see cref="T:Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy" /> is not used by this method
    ///         since the SQL may not be idempotent and does not run in a transaction. An ExecutionStrategy
    ///         can be used explicitly, making sure to also use a transaction if the SQL is not
    ///         idempotent.
    ///     </para>
    ///     <para>
    ///         As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection
    ///         attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional
    ///         arguments. Any parameter values you supply will automatically be converted to a DbParameter -
    ///         <code>context.Database.ExecuteSqlInterpolated($"SELECT * FROM [dbo].[SearchBlogs]({userSuppliedSearchTerm})")</code>.
    ///     </para>
    /// </summary>
    /// <param name="databaseFacade"> The <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade" /> for the context. </param>
    /// <param name="sql"> The interpolated string representing a SQL query with parameters. </param>
    /// <returns> The number of rows affected. </returns>
    int ExecuteSqlInterpolated(System.FormattableString sqlString);

    /// <summary>
    ///     <para>
    ///         Executes the given SQL against the database and returns the number of rows affected.
    ///     </para>
    ///     <para>
    ///         Note that this method does not start a transaction. To use this method with
    ///         a transaction, first call <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.BeginTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.IsolationLevel)" /> or <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.UseTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.Common.DbTransaction)" />.
    ///     </para>
    ///     <para>
    ///         Note that the current <see cref="T:Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy" /> is not used by this method
    ///         since the SQL may not be idempotent and does not run in a transaction. An ExecutionStrategy
    ///         can be used explicitly, making sure to also use a transaction if the SQL is not
    ///         idempotent.
    ///     </para>
    ///     <para>
    ///         As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection
    ///         attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional
    ///         arguments. Any parameter values you supply will automatically be converted to a DbParameter -
    ///         <code>context.Database.ExecuteSqlInterpolatedAsync($"SELECT * FROM [dbo].[SearchBlogs]({userSuppliedSearchTerm})")</code>.
    ///     </para>
    /// </summary>
    /// <param name="databaseFacade"> The <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade" /> for the context. </param>
    /// <param name="sql"> The interpolated string representing a SQL query with parameters. </param>
    /// <param name="cancellationToken"> A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete. </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is the number of rows affected.
    /// </returns>
    Task<int> ExecuteSqlInterpolatedAsync(System.FormattableString sqlString,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     <para>
    ///         Executes the given SQL against the database and returns the number of rows affected.
    ///     </para>
    ///     <para>
    ///         Note that this method does not start a transaction. To use this method with
    ///         a transaction, first call <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.BeginTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.IsolationLevel)" /> or <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.UseTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.Common.DbTransaction)" />.
    ///     </para>
    ///     <para>
    ///         Note that the current <see cref="T:Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy" /> is not used by this method
    ///         since the SQL may not be idempotent and does not run in a transaction. An ExecutionStrategy
    ///         can be used explicitly, making sure to also use a transaction if the SQL is not
    ///         idempotent.
    ///     </para>
    ///     <para>
    ///         As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection
    ///         attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional
    ///         arguments. Any parameter values you supply will automatically be converted to a DbParameter.
    ///         You can also consider using ExecuteSqlInterpolated to use interpolated string syntax to create parameters.
    ///     </para>
    /// </summary>
    /// <param name="databaseFacade"> The <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade" /> for the context. </param>
    /// <param name="sql"> The SQL to execute. </param>
    /// <param name="parameters"> Parameters to use with the SQL. </param>
    /// <returns> The number of rows affected. </returns>
    int ExecuteSqlRaw(string sql, IEnumerable<object> parameters);

    /// <summary>
    ///     <para>
    ///         Executes the given SQL against the database and returns the number of rows affected.
    ///     </para>
    ///     <para>
    ///         Note that this method does not start a transaction. To use this method with
    ///         a transaction, first call <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.BeginTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.IsolationLevel)" /> or <see cref="M:Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.UseTransaction(Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade,System.Data.Common.DbTransaction)" />.
    ///     </para>
    ///     <para>
    ///         Note that the current <see cref="T:Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy" /> is not used by this method
    ///         since the SQL may not be idempotent and does not run in a transaction. An ExecutionStrategy
    ///         can be used explicitly, making sure to also use a transaction if the SQL is not
    ///         idempotent.
    ///     </para>
    ///     <para>
    ///         As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection
    ///         attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional
    ///         arguments. Any parameter values you supply will automatically be converted to a DbParameter.
    ///         You can also consider using ExecuteSqlInterpolated to use interpolated string syntax to create parameters.
    ///     </para>
    /// </summary>
    /// <param name="databaseFacade"> The <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade" /> for the context. </param>
    /// <param name="sql"> The SQL to execute. </param>
    /// <param name="parameters"> Parameters to use with the SQL. </param>
    /// <param name="cancellationToken"> A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete. </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is the number of rows affected.
    /// </returns>
    Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters,
        CancellationToken cancellationToken = default(CancellationToken));
}