using Amazon.S3.Extensions;
using Core.EFCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace Core.EFCore.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddDbContextWithSslOptions<TContext, TConfig>(
        this IServiceCollection services, IConfiguration configuration)  
        where TContext : DbContext 
        where TConfig : DbContextWithSslOptions, new()
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        
        TConfig dbContextOptions = new TConfig();
        var dbContextConfigSection = configuration.GetRequiredSection(dbContextOptions.ConfigSectionName);
        dbContextConfigSection.Bind(dbContextOptions);
        
        string connectionStringName = dbContextOptions.ConnectionStringName;
        dbContextOptions.ConnectionString = configuration.GetConnectionString(connectionStringName)!;

        return services.AddDbContextWithSslCa<TContext, TConfig>(dbContextOptions,
            (provider, config) => provider.GetTlsSslCaFileForRds(config));
    }
    
    /// <summary>
    /// Add the <see cref="TContext"/> to the service collection using the specified configuration.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The db context configuration containing the SSL options.</param>
    /// <param name="getTlsSslCaFileFunc">
    ///     The callback to retrieve the local TLS/SSL CA file, used only if the file does not exist at the path
    ///     specified in the configuration <see cref="DbContextWithSslOptions.CertAuthorityFilePath"/>.
    /// </param>
    private static IServiceCollection AddDbContextWithSslCa<TContext, TConfig>(
        this IServiceCollection services,
        TConfig configuration,
        Func<IServiceProvider, TConfig, string> getTlsSslCaFileFunc)
        where TContext : DbContext
        where TConfig : DbContextWithSslOptions
    {
        if (getTlsSslCaFileFunc == null) throw new ArgumentNullException(nameof(getTlsSslCaFileFunc));

        return services.AddDbContext<TContext>((provider, builder) =>
        {
            int dbConnectionTimeoutInSeconds = configuration.DbCommandTimeoutInSeconds;
            
            if (string.IsNullOrEmpty(configuration.ConnectionString))
                throw new ApplicationException($"No connection string found for {configuration.ConnectionStringName}. Ensure you create a connection string named '{configuration.ConnectionStringName}' in your local secrets.json file, or in parameter store");
            
            var connStringBuilder = new MySqlConnectionStringBuilder(configuration.ConnectionString);
            connStringBuilder.SslMode = configuration.SslMode;
            
            // Get and set certificate chain only if required
            if (configuration.SslMode != MySqlSslMode.None)
            {
                var caFilePath = configuration.CertAuthorityFilePath;
                var caFileInfo = new FileInfo(caFilePath);
                if (!caFileInfo.Exists)
                {
                    // Execute callback to retrieve the file (e.g download from S3...
                    caFilePath = getTlsSslCaFileFunc(provider, configuration);
                }
                connStringBuilder.SslCa = caFilePath;
            }

            connStringBuilder.AutoEnlist = configuration.AutoEnlist;

            string serverVersion = configuration.DatabaseServerVersion;
            builder.UseMySql(connStringBuilder.ConnectionString, ServerVersion.Parse(serverVersion),
                dbContextOptionsBuilder =>
                {
                    dbContextOptionsBuilder.CommandTimeout(dbConnectionTimeoutInSeconds);
                });
        });
    }
    
    /// <summary>
    /// Gets the RDS CA (certificate authority) file from S3 using the specified configuration.
    /// </summary>
    private static string GetTlsSslCaFileForRds(this IServiceProvider provider, DbContextWithSslOptions configuration)
    {
        var filePath = configuration.CertAuthorityFilePath;
        var fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            // Download file from S3 bucket...
            var s3Client = provider.GetRequiredService<Amazon.S3.S3Client>();
            s3Client
                .DownloadAsync(configuration.S3BucketNameForTlsSslCaFile, configuration.S3ObjectKeyForPemFile, filePath)
                .GetAwaiter()
                .GetResult();
        }
        return filePath;
    }
}