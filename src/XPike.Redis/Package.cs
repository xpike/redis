using XPike.IoC;

namespace XPike.Redis
{
    /// <summary>
    /// xPike Caching Dependency Package
    /// </summary>
    public class Package
        : IDependencyPackage
    {
        /// <summary>
        /// Registers xPike Caching dependencies with the Dependency Collection.
        /// </summary>
        /// <param name="dependencyCollection">The IDependencyCollection to register with.</param>
        public void RegisterPackage(IDependencyCollection dependencyCollection) =>
            dependencyCollection.LoadPackage(new Configuration.Package())
                .RegisterSingleton<IRedisConnectionProvider, RedisConnectionProvider>();
    }
}