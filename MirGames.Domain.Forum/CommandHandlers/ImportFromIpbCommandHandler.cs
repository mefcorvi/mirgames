namespace MirGames.Domain.Forum.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.RegularExpressions;

    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handler of re-index forum topics command.
    /// </summary>
    internal sealed class ImportFromIpbCommandHandler : CommandHandler<ImportFromIpbCommand>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The write context factory
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly Lazy<IQueryProcessor> queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFromIpbCommandHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public ImportFromIpbCommandHandler(IReadContextFactory readContextFactory, IWriteContextFactory writeContextFactory, Lazy<IQueryProcessor> queryProcessor)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(queryProcessor != null);

            this.readContextFactory = readContextFactory;
            this.writeContextFactory = writeContextFactory;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Execute(ImportFromIpbCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            int postsCount;

            var smilesRegex =
                new Regex(
                    @"<img src=""style_emoticons/<#EMO_DIR#>/([^""]+)"" style=""vertical-align:middle"" emoid=""([^""]+)"" border=""0"" alt=""([^""]+)"" />",
                    RegexOptions.Compiled);

            var attachmentsRegex = new Regex(@"\[attachmentid=([^\]]+)\]", RegexOptions.Compiled);

            using (var readContext = this.readContextFactory.Create())
            {
                postsCount = readContext.Query<ForumPost>().Count();
            }

            for (int i = 0; i < postsCount; i += 100)
            {
                using (var writeContext = this.writeContextFactory.Create())
                {
                    var posts = writeContext.Set<ForumPost>().OrderBy(p => p.PostId).Skip(i).Take(100);

                    foreach (var post in posts)
                    {
                        post.Text = smilesRegex.Replace(post.Text, "$2");

                        var attachments = attachmentsRegex
                            .Matches(post.Text)
                            .OfType<Match>()
                            .Select(m => int.Parse(m.Groups[1].Value))
                            .ToList();

                        foreach (int attachmentId in attachments)
                        {
                            var attachment = this.queryProcessor.Value.Process(new GetAttachmentInfoQuery { AttachmentId = attachmentId });
                            string attachmentCode = string.Format("[attachmentid={0}]", attachmentId);

                            if (attachment == null)
                            {
                                post.Text = post.Text.Replace(attachmentCode, string.Empty);
                                continue;
                            }

                            if (attachment.IsImage)
                            {
                                post.Text = post.Text.Replace(attachmentCode, string.Format("<img src=\"/Attachment/{0}\" />", attachmentId));
                            }
                            else
                            {
                                post.Text = post.Text.Replace(
                                    attachmentCode,
                                    string.Format("<a href=\"/Attachment/{0}\">{1}</a>", attachmentId, attachment.FileName));
                            }
                        }
                    }

                    writeContext.SaveChanges();
                }
            }
        }
    }
}