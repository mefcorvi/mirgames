// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="BlogResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.Services
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;

    /// <summary>
    /// Resolves information about blogs.
    /// </summary>
    internal sealed class BlogResolver : IBlogResolver
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The cache manager factory.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogResolver" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public BlogResolver(IReadContextFactory readContextFactory, ICacheManagerFactory cacheManagerFactory)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(cacheManagerFactory != null);

            this.readContextFactory = readContextFactory;
            this.cacheManager = cacheManagerFactory.Create("BlogResolver");
        }

        /// <inheritdoc />
        public BlogViewModel Resolve(BlogViewModel blog)
        {
            if (blog == null || !blog.BlogId.HasValue)
            {
                return blog;
            }

            return this.cacheManager.GetOrAdd(
                blog.BlogId.ToString(),
                () =>
                {
                    using (var readContext = this.readContextFactory.Create())
                    {
                        var loadedBlog = readContext.Query<Blog>().FirstOrDefault(b => b.Id == blog.BlogId);

                        if (loadedBlog == null)
                        {
                            return null;
                        }

                        return new BlogViewModel
                        {
                            BlogId = loadedBlog.Id,
                            Description = loadedBlog.Description,
                            EntityId = loadedBlog.EntityId,
                            EntityType = loadedBlog.EntityType,
                            Title = loadedBlog.Title
                        };
                    }
                },
                DateTimeOffset.Now.AddDays(1));
        }
    }
}