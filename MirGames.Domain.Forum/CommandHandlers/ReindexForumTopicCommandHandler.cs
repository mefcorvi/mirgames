// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ReindexForumTopicCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Security.Claims;
    using System.Text;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handler of re-index forum topics command.
    /// </summary>
    internal sealed class ReindexForumTopicCommandHandler : CommandHandler<ReindexForumTopicCommand>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexForumTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public ReindexForumTopicCommandHandler(ISearchEngine searchEngine, IQueryProcessor queryProcessor)
        {
            this.searchEngine = searchEngine;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Execute(ReindexForumTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            var posts = this.queryProcessor.Process(new GetForumTopicPostsQuery { TopicId = command.TopicId });
            var sb = new StringBuilder();

            foreach (var post in posts)
            {
                sb.AppendLine(post.Text);
            }

            this.searchEngine.Remove(command.TopicId, "ForumTopic");
            this.searchEngine.Index(command.TopicId, "ForumTopic", sb.ToString());
        }
    }
}