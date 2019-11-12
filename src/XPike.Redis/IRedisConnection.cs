using System.Threading.Tasks;
using StackExchange.Redis;

namespace XPike.Redis
{
    /// <summary>
    /// Encapsulates and manages an actual connection to Redis.
    /// This is a parallel to the IConnectionMultiplexer in StackExchange.Redis.
    /// </summary>
    public interface IRedisConnection
    {
        /// <summary>
        /// The name of the connection in the application settings.
        /// </summary>
        string ConnectionName { get; }

        /// <summary>
        /// Establishes a connection to the Redis server/cluster.
        /// </summary>
        /// <returns></returns>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Returns the Connection Multiplexer for the established connection.
        /// </summary>
        /// <returns></returns>
        Task<IConnectionMultiplexer> GetConnectionMultiplexerAsync();
        
        /// <summary>
        /// Gets the Database (Cache) from the Connection Multiplexer.
        /// </summary>
        /// <returns></returns>
        Task<IDatabase> GetDatabaseAsync();

        /// <summary>
        /// Gets a Subscriber (for pub/sub - it is also used to publish) from the Connection Multiplexer.
        /// </summary>
        /// <returns></returns>
        Task<ISubscriber> GetSubscriberAsync();
    }
}