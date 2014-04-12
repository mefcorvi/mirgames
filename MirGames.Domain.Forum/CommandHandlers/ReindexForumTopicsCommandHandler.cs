// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ReindexForumTopicsCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handler of re-index forum topics command.
    /// </summary>
    internal sealed class ReindexForumTopicsCommandHandler : CommandHandler<ReindexForumTopicsCommand>
    {
        /// <summary>
        /// The write context factory
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexForumTopicsCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ReindexForumTopicsCommandHandler(IWriteContextFactory writeContextFactory, Lazy<ICommandProcessor> commandProcessor)
        {
            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Execute(ReindexForumTopicsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            ICollection<ForumTopic> topics;

            using (var writeContext = this.writeContextFactory.Create())
            {
                topics = writeContext.Set<ForumTopic>().ToList();
                
                foreach (var topic in topics)
                {
                    topic.PostsCount = writeContext.Set<ForumPost>().Count(p => p.TopicId == topic.TopicId);

                    var lastPost = writeContext
                        .Set<ForumPost>()
                        .Where(p => p.TopicId == topic.TopicId)
                        .OrderByDescending(p => p.CreatedDate)
                        .FirstOrDefault();

                    if (lastPost != null)
                    {
                        topic.LastPostAuthorId = lastPost.AuthorId;
                        topic.LastPostAuthorLogin = lastPost.AuthorLogin;
                        topic.UpdatedDate = lastPost.CreatedDate;
                    }
                }

                writeContext.SaveChanges();
            }

            foreach (var topic in topics)
            {
                this.commandProcessor.Value.Execute(new ReindexForumTopicCommand { TopicId = topic.TopicId });
            }

            this.ReindexTags(topics);
        }

        /// <summary>
        /// Re-indexes the tags.
        /// </summary>
        /// <param name="topics">The topics.</param>
        private void ReindexTags(IEnumerable<ForumTopic> topics)
        {
            foreach (var topic in topics)
            {
                using (var writeContext = this.writeContextFactory.Create())
                {
                    int topicId = topic.TopicId;
                    
                    var tags = writeContext.Set<ForumTag>().Where(t => t.TopicId == topicId);
                    writeContext.Set<ForumTag>().RemoveRange(tags);
                    writeContext.SaveChanges();

                    foreach (var tag in topic.TagsList.Split(','))
                    {
                        writeContext.Set<ForumTag>().Add(
                            new ForumTag
                                {
                                    TagText = tag.Trim(),
                                    TopicId = topicId
                                });
                    }

                    writeContext.SaveChanges();
                }
            }
        }
    }
}