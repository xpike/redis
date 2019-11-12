using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using XPike.Logging;
using XPike.Settings;

namespace XPike.Redis
{
    /// <inheritdoc cref="IRedisConnectionProvider" />
    /// <summary>
    /// Manages multiple connections to different Redis servers/clusters, isolated by connection name.
    /// Uses RedisConnection instances for its underlying implementation.
    ///
    /// This is intended to be consumed by an IRedisCacheConnectionProvider or IRedisEventBusConnectionProvider.
    /// This is intended to be a Dependency Injection managed Singleton.
    /// </summary>
    public class RedisConnectionProvider
        : IRedisConnectionProvider
    {
        public const string DEFAULT_CONNECTION_NAME = "default";

        private readonly ConcurrentDictionary<string, IRedisConnection> _connections = new ConcurrentDictionary<string, IRedisConnection>();
        private readonly ISettings<RedisSettings> _settings;
        private readonly ILog<RedisConnectionProvider> _myLogger;
        private readonly ILog<RedisConnection> _connectionLogger;

        /// <summary>
        /// Creates a new RedisConnectionProvider.
        /// </summary>
        /// <param name="settings">The RedisSettings to use.</param>
        /// <param name="logService">The ILogService to provide to the </param>
        public RedisConnectionProvider(ISettings<RedisSettings> settings, ILog<RedisConnectionProvider> myLogger, ILog<RedisConnection> connectionLogger)
        {
            _settings = settings;
            _myLogger = myLogger;
            _connectionLogger = connectionLogger;
        }

        public async Task<IRedisConnection> GetConnectionAsync(string connectionName, TimeSpan? timeout = null, CancellationToken? ct = null)
        {
            if (connectionName == null)
                return await GetConnectionAsync(DEFAULT_CONNECTION_NAME, timeout, ct);

            if (_connections.TryGetValue(connectionName, out var connection))
                return connection;

            if (!_settings.Value.Connections.ContainsKey(connectionName))
            {
                if (connectionName == DEFAULT_CONNECTION_NAME)
                    throw new InvalidOperationException("No default Redis connection configuration found!");

                _myLogger.Warn($"No Redis configuration found for connection '{connectionName}' - using default connection instead.");

                connection = await GetConnectionAsync(DEFAULT_CONNECTION_NAME, timeout, ct);
            }
            else
            {
                connection = new RedisConnection(connectionName, _settings, _connectionLogger);

                if (!await connection.ConnectAsync())
                    _myLogger.Warn($"Failed to connect to Redis connection '{connectionName}': Provider returned false.");
            }

            _connections[connectionName] = connection;

            return connection;
        }
    }
}