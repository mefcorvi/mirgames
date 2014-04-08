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
    internal sealed class DeleteForumPostCommandHandler : CommandHandler<DeleteForumPostCommand>
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
        /// Initializes a new instance of the <see cref="DeleteForumPostCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public DeleteForumPostCommandHandler(
            IWriteContextFactory writeContextFactory,
            IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(DeleteForumPostCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            ForumPost post;

            using (var writeContext = this.writeContextFactory.Create())
            {
                post = writeContext.Set<ForumPost>().FirstOrDefault(t => t.PostId == command.PostId);

                if (post == null)
                {
                    throw new ItemNotFoundException("Post", command.PostId);
                }

                authorizationManager.EnsureAccess(principal, "Delete", post);
                
                writeContext.Set<ForumPost>().Remove(post);
                writeContext.SaveChanges();

                var topic = writeContext.Set<ForumTopic>().First(t => t.TopicId == post.TopicId);
                topic.PostsCount = writeContext.Set<ForumPost>().Count(p => p.TopicId == post.TopicId);

                var lastPost = writeContext.Set<ForumPost>().Where(p => p.TopicId == post.TopicId).OrderByDescending(p => p.PostId).First();
                topic.LastPostAuthorId = lastPost.AuthorId;
                topic.LastPostAuthorLogin = lastPost.AuthorLogin;
                topic.UpdatedDate = lastPost.CreatedDate;

                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new ForumPostDeletedEvent { PostId = command.PostId, TopicId = post.TopicId });
        }
    }
}