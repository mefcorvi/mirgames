// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UpgradeForumCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.TextTransform;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class UpgradeForumCommandHandler : CommandHandler<UpgradeForumCommand>
    {
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeForumCommandHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventLog">The event log.</param>
        public UpgradeForumCommandHandler(
            IReadContextFactory readContextFactory,
            IWriteContextFactory writeContextFactory,
            IEventLog eventLog)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventLog != null);

            this.readContextFactory = readContextFactory;
            this.writeContextFactory = writeContextFactory;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        protected override void Execute(UpgradeForumCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            bool topicsEnd = false;
            int currentPage = 0;
            var shortTextExtractor = new ShortTextExtractor();

            while (!topicsEnd)
            {
                using (var writeContext = this.writeContextFactory.Create())
                {
                    var posts = writeContext.Set<ForumPost>();
                    var topics = writeContext
                        .Set<ForumTopic>()
                        .OrderBy(t => t.TopicId)
                        .Skip(currentPage * 20)
                        .Take(20)
                        .GroupJoin(
                            posts,
                            t => t.TopicId,
                            p => p.TopicId,
                            (topic, enumerable) => new
                            {
                                Topic = topic,
                                Post = enumerable.OrderBy(p => p.PostId).FirstOrDefault()
                            })
                        .ToArray();

                    topics.Where(t => t.Post != null).ForEach(t => t.Topic.ShortDescription = HttpUtility.HtmlDecode(shortTextExtractor.Transform(t.Post.SourceText)));
                    writeContext.SaveChanges();

                    currentPage++;
                    topicsEnd = topics.Length == 0;
                }
            }
        }
    }
}