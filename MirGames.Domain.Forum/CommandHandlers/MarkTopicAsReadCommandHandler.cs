namespace MirGames.Domain.Forum.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class MarkTopicAsReadCommandHandler : CommandHandler<MarkTopicAsReadCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkTopicAsReadCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public MarkTopicAsReadCommandHandler(IWriteContextFactory writeContextFactory, IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(MarkTopicAsReadCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            int userId = principal.GetUserId().GetValueOrDefault();

            using (var writeContext = this.writeContextFactory.Create())
            {
                var topic = writeContext.Set<ForumTopic>().FirstOrDefault(t => t.TopicId == command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                authorizationManager.EnsureAccess(principal, "MarkAsRead", topic);

                var isAlreadyMarked = writeContext.Set<ForumTopicRead>().Any(
                    t =>
                    t.UserId == userId
                    && command.TopicId >= t.StartTopicId && t.EndTopicId >= command.TopicId);

                if (isAlreadyMarked)
                {
                    return;
                }

                var siblings = writeContext.Set<ForumTopicRead>().Where(
                    t =>
                    t.UserId == userId
                    && (t.StartTopicId == (command.TopicId + 1) || t.EndTopicId == (command.TopicId - 1))).OrderBy(t => t.StartTopicId).ToList();

                if (siblings.Count > 2)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Three or more intersections of topics range have been found for topic #{0}", topic.TopicId));
                }

                foreach (ForumTopicRead forumTopicRead in siblings)
                {
                    if (forumTopicRead.StartTopicId == command.TopicId + 1)
                    {
                        forumTopicRead.StartTopicId = command.TopicId;
                    }
                    
                    if (forumTopicRead.EndTopicId == command.TopicId - 1)
                    {
                        forumTopicRead.EndTopicId = command.TopicId;
                    }
                }

                if (siblings.Count == 2 && siblings[0].EndTopicId == siblings[1].StartTopicId)
                {
                    siblings[0].EndTopicId = siblings[1].EndTopicId;
                    writeContext.Set<ForumTopicRead>().Remove(siblings[1]);
                }

                if (siblings.Count == 0)
                {
                    writeContext.Set<ForumTopicRead>().Add(
                        new ForumTopicRead
                        {
                            StartTopicId = command.TopicId,
                            EndTopicId = command.TopicId,
                            UserId = userId
                        });
                }

                var oldUnreadTopics = writeContext.Set<ForumTopicUnread>().Where(u => u.UserId == userId && u.TopicId == command.TopicId);
                writeContext.Set<ForumTopicUnread>().RemoveRange(oldUnreadTopics);

                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new ForumTopicReadEvent { TopicId = command.TopicId, UserIdentifiers = new[] { userId } });
        }
    }
}