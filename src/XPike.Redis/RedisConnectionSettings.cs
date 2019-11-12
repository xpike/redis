namespace XPike.Redis
{
    /// <summary>
    /// Settings for an individual Redis server/cluster.
    /// </summary>
    public class RedisConnectionSettings
    {
        /// <summary>
        /// The Connection String to use.
        /// The format used by StackExchange.Redis (compatible with AWS and Azure) is expected.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Controls if this connection is enabled.
        /// If set to false, connections will not be established - the IRedisConnection will return only "false" or "null" from its methods.
        /// </summary>
        public bool Enabled { get; set; }
    }
}