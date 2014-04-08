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
    internal sealed class RemoveAttachmentsCommandHandler : CommandHandler<RemoveAttachmentsCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveAttachmentsCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public RemoveAttachmentsCommandHandler(IWriteContextFactory writeContextFactory)
        {
            Contract.Requires(writeContextFactory != null);

            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Execute(RemoveAttachmentsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                var attachments = writeContext.Set<Attachment>().Where(x => x.EntityId == command.EntityId && x.EntityType == command.EntityType).ToList();

                foreach (var attachment in attachments)
                {
                    authorizationManager.EnsureAccess(principal, "Remove", attachment);
                    attachment.IsPublished = false;
                }

                writeContext.SaveChanges();
            }
        }
    }
}