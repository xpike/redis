using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using XPike.Configuration;
using XPike.Logging;

namespace XPike.Redis
{
    /// <inheritdoc cref="IRedisConnection" />
    /// <summary>
    /// Establishes connections with Redis servers/clusters using the StackExchange.Redis library.
    /// </summary>
    public class RedisConnection
        : IRedisConnection
    {
        private readonly IConfig<RedisConfig> _config;
        private readonly ILog<RedisConnection> _logger;

        private IConnectionMultiplexer _multiplexer;

        public string ConnectionName { get; }

        /// <summary>
        /// Create a new Redis Connection.
        /// This does not establish the connection.
        /// 
        /// This is intended to only be called from an IRedisConnectionProvider.
        /// </summary>
        /// <param name="connectionName">The name of the connection to load from configuration.</param>
        /// <param name="config">The RedisConfig to use.</param>
        /// <param name="logger">The ILogService to log exceptions and connection information to.</param>
        public RedisConnection(string connectionName, IConfig<RedisConfig> config, ILog<RedisConnection> logger)
        {
            ConnectionName = connectionName;
            _config = config;
            _logger = logger;
        }

        private RedisConnectionConfig GetConfig() =>
            _config.CurrentValue.Connections[ConnectionName];

        public async Task<bool> ConnectAsync()
        {
            var config = GetConfig();

            if (!config.Enabled)
                return false;

            try
            {
                var options = ConfigurationOptions.Parse(config.ConnectionString);

                var sb = new StringBuilder();
                using (var logWriter = new StringWriter(sb))
                {
                    _multiplexer = await ConnectionMultiplexer.ConnectAsync(options, logWriter).ConfigureAwait(false);

                    _logger.Info($"Redis Connection '{ConnectionName}' Connection Log: {sb}");
                }
                sb.Clear();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to connect to Redis at {config.ConnectionString}: {ex.Message} ({ex.GetType()})",
                    ex,
                    new Dictionary<string, string>
                    {
                        {"configuration", JsonConvert.SerializeObject(config)}
                    });

                return false;
            }
        }

        public Task<IConnectionMultiplexer> GetConnectionMultiplexerAsync() =>
            Task.FromResult(_multiplexer);

        public Task<IDatabase> GetDatabaseAsync() =>
            Task.Run(() => _multiplexer?.GetDatabase());

        public Task<ISubscriber> GetSubscriberAsync() =>
            Task.Run(() => _multiplexer?.GetSubscriber());
    }
}