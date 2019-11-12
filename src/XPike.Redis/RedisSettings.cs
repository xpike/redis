using System.Collections.Generic;

namespace XPike.Redis
{
    /// <summary>
    /// Settings for xPike Redis.
    /// </summary>
    public class RedisSettings
    {
        /// <summary>
        /// The set of configured connections.
        /// </summary>
        public Dictionary<string, RedisConnectionSettings> Connections { get; set; }
    }
}