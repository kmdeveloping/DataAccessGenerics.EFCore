using MySqlConnector;
using Newtonsoft.Json;

namespace Core.EFCore.Configuration;

public abstract class DbContextWithSslOptions
{
    /// <summary> The connection string used to connect to the database. </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary> The name of the database connection string in the configuration. </summary>
    public abstract string ConnectionStringName { get; }

    public abstract string ConfigSectionName { get; }

    /// <summary> Gets or sets the number of seconds before a database command times out. </summary>
    public int DbCommandTimeoutInSeconds { get; set; } = 30;
    public string DatabaseServerVersion { get; set; } = "8.0.23";

    /// <summary> Gets or sets a boolean value that indicates if an opened connection should participate in the current scope. </summary>
    /// <remarks> Default value is <c>true</c>.</remarks>
    public bool AutoEnlist { get; set; } = true;

    /// <summary> Indicates whether to use SSL connections and how to handle server certificate errors. </summary>
    /// <remarks> Default value is <see cref="Required"/>.</remarks>
    public MySqlSslMode SslMode { get; set; } = MySqlSslMode.Preferred;
        
    /// <summary> Path to a local file that contains a list of trusted TLS/SSL CAs. </summary>
    /// <remarks> If the file does not already exist, this will be used to save the file to this location. </remarks>
    public string CertAuthorityFilePath { get; set; } = null!;

    /// <summary> Gets or sets the S3 bucket name in which to store/retrieve the RDS certificate chain file. </summary>
    public string S3BucketNameForTlsSslCaFile { get; set; } = null!;

    /// <summary> Gets or sets the S3 object key of the RDS certificate chain file (usually the file name). </summary>
    public string S3ObjectKeyForPemFile { get; set; } = null!;
}