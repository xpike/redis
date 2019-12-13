using System.Collections.Generic;

namespace XPike.Redis
{
    /// <summary>
    /// Configuration for xPike Redis.
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// The set of configured connections.
        /// </summary>
        public Dictionary<string, RedisConnectionConfig> Connections { get; set; }
    }
}