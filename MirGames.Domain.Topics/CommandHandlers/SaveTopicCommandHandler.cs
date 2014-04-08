namespace MirGames.Domain.Topics.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class SaveCommandHandler : CommandHandler<SaveTopicCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The text hash provider.
        /// </summary>
        private readonly ITextHashProvider textHashProvider;

        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The topics text transform.
        /// </summary>
        private readonly ITextTransform topicsTextTransform;

        /// <summary>
        /// The short text extractor.
        /// </summary>
        private readonly IShortTextExtractor shortTextExtractor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="textHashProvider">The text hash provider.</param>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="shortTextExtractor">The short text extractor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textTransform">The text transform.</param>
        public SaveCommandHandler(IWriteContextFactory writeContextFactory, ITextHashProvider textHashProvider, ISearchEngine searchEngine, IShortTextExtractor shortTextExtractor, ICommandProcessor commandProcessor, ITextTransform textTransform)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(textHashProvider != null);
            Contract.Requires(searchEngine != null);
            Contract.Requires(shortTextExtractor != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.textHashProvider = textHashProvider;
            this.searchEngine = searchEngine;
            this.shortTextExtractor = shortTextExtractor;
            this.commandProcessor = commandProcessor;
            this.topicsTextTransform = textTransform;
        }

        /// <inheritdoc />
        public override void Execute(SaveTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            Topic topic;

            using (var writeContext = this.writeContextFactory.Create())
            {
                topic = writeContext.Set<Topic>().SingleOrDefault(t => t.Id == command.TopicId);
                authorizationManager.EnsureAccess(principal, "Edit", topic);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                topic.TextHash = this.textHashProvider.GetHash(command.Text);
                topic.TopicTitle = command.Title;
                topic.TagsList = command.Tags;
                topic.EditDate = DateTime.UtcNow;
                topic.UserIp = principal.GetHostAddress();

                foreach (var oldTag in writeContext.Set<TopicTag>().Where(t => t.TopicId == command.TopicId))
                {
                    writeContext.Set<TopicTag>().Remove(oldTag);
                }

                var topicContent = writeContext.Set<TopicContent>().Single(content => content.TopicId == command.TopicId);
                topicContent.TopicText = this.topicsTextTransform.Transform(command.Text);
                topicContent.TopicTextSource = command.Text;
                topicContent.TopicTextShort = this.topicsTextTransform.Transform(this.shortTextExtractor.Extract(command.Text));
                
                foreach (var tag in command.Tags.Split(','))
                {
                    writeContext.Set<TopicTag>().Add(new TopicTag { TagText = tag.Trim(), TopicId = command.TopicId });
                }

                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = topic.Id,
                            Identifiers = command.Attachments
                        });
            }

            this.searchEngine.Remove(topic.Id, "Topic");
            this.searchEngine.Index(topic.Id, "Topic", command.Title + " " + command.Text + " " + command.Tags);
        }
    }
}