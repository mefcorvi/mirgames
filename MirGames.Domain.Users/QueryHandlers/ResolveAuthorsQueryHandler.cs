namespace MirGames.Domain.Users.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetUsersQuery.
    /// </summary>
    internal sealed class ResolveAuthorsQueryHandler : SingleItemQueryHandler<ResolveAuthorsQuery, IEnumerable<AuthorViewModel>>
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// The avatar provider.
        /// </summary>
        private readonly IAvatarProvider avatarProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveAuthorsQueryHandler" /> class.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="avatarProvider">The avatar provider.</param>
        public ResolveAuthorsQueryHandler(ICacheManager cacheManager, IAvatarProvider avatarProvider)
        {
            this.cacheManager = cacheManager;
            this.avatarProvider = avatarProvider;
        }

        /// <inheritdoc />
        public override IEnumerable<AuthorViewModel> Execute(IReadContext readContext, ResolveAuthorsQuery query, ClaimsPrincipal principal)
        {
            Contract.Requires(query.Authors != null);

            var resolvedUsers = new List<AuthorViewModel>();
            var userIdentifiers = new List<int>();
            var requestedUsers = query.Authors.EnsureCollection();

            var requestedIdentifiers = requestedUsers.Where(x => x.Id.HasValue).Select(x => x.Id).Cast<int>().Distinct();

            foreach (var userId in requestedIdentifiers)
            {
                string cacheKey = GetCacheKey(userId);

                if (this.cacheManager.Contains(cacheKey))
                {
                    resolvedUsers.Add(this.cacheManager.Get<AuthorViewModel>(cacheKey));
                }
                else
                {
                    userIdentifiers.Add(userId);
                }
            }

            if (userIdentifiers.Any())
            {
                resolvedUsers.AddRange(
                    this.GetUsersQuery(readContext, userIdentifiers)
                    .Select(x => new 
                        {
                            x.AvatarUrl,
                            x.Mail,
                            x.Login,
                            x.Id
                        })
                        .ToList()
                        .Select(
                            x => new AuthorViewModel
                            {
                                AvatarUrl = x.AvatarUrl ?? this.avatarProvider.GetAvatarUrl(x.Mail, x.Login),
                                Id = x.Id,
                                Login = x.Login
                            }));
            }

            foreach (var user in resolvedUsers)
            {
                Debug.Assert(user.Id != null, "user.Id != null");
                this.cacheManager.AddOrUpdate(GetCacheKey(user.Id.Value), user, DateTimeOffset.Now.AddMinutes(15));
            }

            var dictionary = resolvedUsers.ToDictionary(item => item.Id);

            foreach (var author in requestedUsers)
            {
                if (author.Id.HasValue && dictionary.ContainsKey(author.Id.Value))
                {
                    var resolvedAuthor = dictionary[author.Id.Value];
                    author.AvatarUrl = resolvedAuthor.AvatarUrl;
                    author.Login = resolvedAuthor.Login;
                }
                else
                {
                    author.AvatarUrl = this.avatarProvider.GetAvatarUrl(null, author.Login);
                }
            }

            return requestedUsers;
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>The cache key.</returns>
        private static string GetCacheKey(int userId)
        {
            return "GetAuthorsQueryHandler.User" + userId;
        }

        /// <summary>
        /// Gets the users query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>The users query.</returns>
        private IQueryable<User> GetUsersQuery(IReadContext readContext, IEnumerable<int> identifiers)
        {
            return readContext.Query<User>().Where(x => identifiers.Contains(x.Id)).OrderBy(x => x.Id);
        }
    }
}