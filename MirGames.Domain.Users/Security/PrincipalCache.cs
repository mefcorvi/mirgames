namespace MirGames.Domain.Users.Security
{
    using System;
    using System.Security.Claims;

    using MirGames.Infrastructure.Cache;

    /// <summary>
    /// Stores principal in cache.
    /// </summary>
    internal sealed class PrincipalCache : IPrincipalCache
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalCache"/> class.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        public PrincipalCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetOrAdd(string sessionId, Func<ClaimsPrincipal> principalFactory)
        {
            return this.cacheManager.GetOrAdd(this.GetKey(sessionId), principalFactory, DateTimeOffset.Now.AddMinutes(5));
        }

        /// <inheritdoc />
        public void Remove(string sessionId)
        {
            this.cacheManager.Remove(this.GetKey(sessionId));
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns>The cache key.</returns>
        private string GetKey(string sessionId)
        {
            return string.Format("StringPrincipal{0}", sessionId);
        }
    }
}