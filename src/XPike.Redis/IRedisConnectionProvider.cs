using System;
using System.Threading;
using System.Threading.Tasks;

namespace XPike.Redis
{
    /// <summary>
    /// Manages a collection of Redis Connections, isolated by connection name, to different servers/clusters.
    /// </summary>
    public interface IRedisConnectionProvider
    {
        /// <summary>
        /// Gets and/or establishes a connection using the specified connection name.
        /// Configuration values are loaded from application settings.
        /// 
        /// If no configuration for the specified connection name is found, the default connection will be used.
        /// If no default configuration is found, an InvalidOperationException will be thrown.
        /// </summary>
        /// <param name="connectionName">The name of the connection to use when loading settings from application configuration.</param>
        /// <param name="timeout">An optional timeout.  By default, StackExchange.Redis uses a 5-second timeout when creating a connection.</param>
        /// <param name="ct">An optional Cancellation Token to abort the connection.</param>
        /// <returns></returns>
        Task<IRedisConnection> GetConnectionAsync(string connectionName, TimeSpan? timeout = null, CancellationToken? ct = null);
    }
}