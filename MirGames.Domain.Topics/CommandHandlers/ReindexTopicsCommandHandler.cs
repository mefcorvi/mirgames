// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ReindexTopicsCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Data.Entity;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handler of re-index topics command.
    /// </summary>
    public class ReindexTopicsCommandHandler : CommandHandler<ReindexTopicsCommand>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The write context factory
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexTopicsCommandHandler" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        public ReindexTopicsCommandHandler(ISearchEngine searchEngine, IWriteContextFactory writeContextFactory)
        {
            this.searchEngine = searchEngine;
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        protected override void Execute(ReindexTopicsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            this.searchEngine.ClearIndex();
            using (var writeContext = this.writeContextFactory.Create())
            {
                var topics = writeContext.Set<Topic>().Include(t => t.Content);
                
                foreach (var topic in topics)
                {
                    this.searchEngine.Remove(topic.Id, "Topic");
                    this.searchEngine.Index(
                        topic.Id,
                        "Topic",
                        topic.TopicTitle + " " + topic.Content.TopicText,
                        new SearchIndexTerm("tags", topic.TagsList) { IsIndexed = true, IsNormalized = false });
                }
            }
        }
    }
}