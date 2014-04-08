namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class DeleteCommentCommandHandler : CommandHandler<DeleteCommentCommand>
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
        /// Initializes a new instance of the <see cref="DeleteCommentCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        public DeleteCommentCommandHandler(IWriteContextFactory writeContextFactory, IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(DeleteCommentCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            Comment comment;
            
            using (var writeContext = this.writeContextFactory.Create())
            {
                comment = writeContext.Set<Comment>().FirstOrDefault(c => c.CommentId == command.CommentId);

                if (comment == null)
                {
                    throw new ItemNotFoundException("Comment", command.CommentId);
                }

                authorizationManager.EnsureAccess(principal, "Delete", comment);
                writeContext.Set<Comment>().Remove(comment);

                var topic = writeContext.Set<Topic>().FirstOrDefault(t => t.Id == comment.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", comment.TopicId);
                }

                topic.CountComment = topic.CountComment - 1;
                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new CommentDeletedEvent { CommentId = comment.CommentId, TopicId = comment.TopicId });
        }
    }
}