using XPike.IoC;

namespace XPike.Redis
{
    /// <summary>
    /// Extension method to register xPike Redis dependencies in an IoC container.
    /// </summary>
    public static class IDependencyCollectionExtensions
    {
        /// <summary>
        /// Adds xPike Redis dependencies to an IoC container.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDependencyCollection AddXPikeRedis(this IDependencyCollection collection) =>
            collection.LoadPackage(new XPike.Redis.Package());
    }
}