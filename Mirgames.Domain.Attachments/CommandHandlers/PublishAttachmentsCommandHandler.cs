namespace MirGames.Domain.Attachments.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Attachments.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the publishing of attachments
    /// </summary>
    internal sealed class PublishAttachmentsCommandHandler : CommandHandler<PublishAttachmentsCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishAttachmentsCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public PublishAttachmentsCommandHandler(IWriteContextFactory writeContextFactory)
        {
            Contract.Requires(writeContextFactory != null);

            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Execute(PublishAttachmentsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command.Identifiers != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var identifiers = command.Identifiers.ToArray();
                var attachments = writeContext.Set<Attachment>().Where(x => identifiers.Contains(x.AttachmentId) && !x.IsPublished).ToList();

                foreach (var attachment in attachments)
                {
                    authorizationManager.EnsureAccess(principal, "Publish", attachment);
                    attachment.IsPublished = true;
                    attachment.EntityId = command.EntityId;
                }

                writeContext.SaveChanges();
            }
        }
    }
}