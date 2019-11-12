using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using XPike.Logging;
using XPike.Settings;

namespace XPike.Redis
{
    /// <inheritdoc cref="IRedisConnection" />
    /// <summary>
    /// Establishes connections with Redis servers/clusters using the StackExchange.Redis library.
    /// </summary>
    public class RedisConnection
        : IRedisConnection
    {
        private readonly ISettings<RedisSettings> _settings;
        private readonly ILog<RedisConnection> _logger;

        private IConnectionMultiplexer _multiplexer;

        public string ConnectionName { get; }

        /// <summary>
        /// Create a new Redis Connection.
        /// This does not establish the connection.
        /// 
        /// This is intended to only be called from an IRedisConnectionProvider.
        /// </summary>
        /// <param name="connectionName">The name of the connection settings to load from configuration.</param>
        /// <param name="settings">The RedisSettings to use.</param>
        /// <param name="logger">The ILogService to log exceptions and connection information to.</param>
        public RedisConnection(string connectionName, ISettings<RedisSettings> settings, ILog<RedisConnection> logger)
        {
            ConnectionName = connectionName;
            _settings = settings;
            _logger = logger;
        }

        private RedisConnectionSettings GetSettings() =>
            _settings.Value.Connections[ConnectionName];

        public async Task<bool> ConnectAsync()
        {
            var settings = GetSettings();

            if (!settings.Enabled)
                return false;

            try
            {
                var options = ConfigurationOptions.Parse(settings.ConnectionString);

                var sb = new StringBuilder();
                using (var logWriter = new StringWriter(sb))
                {
                    _multiplexer = await ConnectionMultiplexer.ConnectAsync(options, logWriter);

                    _logger.Info($"Redis Connection '{ConnectionName}' Connection Log: {sb}");
                }
                sb.Clear();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to connect to Redis at {settings.ConnectionString}: {ex.Message} ({ex.GetType()})",
                    ex,
                    new Dictionary<string, string>
                    {
                        {"settings", JsonConvert.SerializeObject(settings)}
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