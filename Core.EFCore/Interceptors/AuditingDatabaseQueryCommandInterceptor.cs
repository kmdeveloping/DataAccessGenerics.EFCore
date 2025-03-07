using System.Data.Common;
using CqrsFramework.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.EFCore.Interceptors;

public class AuditingDatabaseQueryCommandInterceptor(ILogger logger) : DbCommandInterceptor
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executing scalar");
        return base.ScalarExecuting(command, eventData, result);
    }

    public override async ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, 
        CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executing scalar");
        return await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executed scalar in {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return base.ScalarExecuted(command, eventData, result);
    }

    public override async ValueTask<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result,
        CancellationToken cancellationToken = default)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executed scalar in {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return await base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executing non-query");
        return base.NonQueryExecuting(command, eventData, result);
    }
    
    public override async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executing non-query");
        return await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executed non-query in {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return base.NonQueryExecuted(command, eventData, result);
    }

    public override async ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executed non-query in {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executing reader");
        return base.ReaderExecuting(command, eventData, result);
    }
    
    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executing reader");
        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executed reader in {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return base.ReaderExecuted(command, eventData, result);
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("Executed reader in {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override async Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, 
        CancellationToken cancellationToken = default)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Error(eventData.Exception, "SQL command execution failed after {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);

        await base.CommandFailedAsync(command, eventData, cancellationToken);
    }

    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Error(eventData.Exception, "SQL command execution failed after {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        base.CommandFailed(command, eventData);
    }
    
    public override void CommandCanceled(DbCommand command, CommandEndEventData eventData)
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("SQL command canceled after {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        base.CommandCanceled(command, eventData);
    }

    public override Task CommandCanceledAsync(DbCommand command, CommandEndEventData eventData, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger
            .ForContext("CommandText", command.CommandText)
            .ForContext("Parameters", command.Parameters, true)
            .Debug("SQL command canceled after {ElapsedMilliseconds}ms", eventData.Duration.TotalMilliseconds);
        return base.CommandCanceledAsync(command, eventData, cancellationToken);
    }
}