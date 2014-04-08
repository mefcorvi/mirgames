namespace MirGames.Domain.Topics.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
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
    internal sealed class AddNewTopicCommandHandler : CommandHandler<AddNewTopicCommand, int>
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
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

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
        /// Initializes a new instance of the <see cref="AddNewTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="textHashProvider">The text hash provider.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="shortTextExtractor">The short text extractor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textTransform">The text transform.</param>
        public AddNewTopicCommandHandler(
            IWriteContextFactory writeContextFactory,
            ITextHashProvider textHashProvider,
            IEventBus eventBus,
            IShortTextExtractor shortTextExtractor,
            ICommandProcessor commandProcessor,
            ITextTransform textTransform)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(textHashProvider != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(shortTextExtractor != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(textTransform != null);

            this.writeContextFactory = writeContextFactory;
            this.textHashProvider = textHashProvider;
            this.eventBus = eventBus;
            this.shortTextExtractor = shortTextExtractor;
            this.commandProcessor = commandProcessor;
            this.topicsTextTransform = textTransform;
        }

        /// <inheritdoc />
        public override int Execute(AddNewTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            var topic = new Topic
            {
                AuthorId = principal.GetUserId().GetValueOrDefault(),
                CountComment = 0,
                TextHash = this.textHashProvider.GetHash(command.Text),
                TopicTitle = command.Title,
                TagsList = command.Tags,
                CreationDate = DateTime.UtcNow,
                UserIp = principal.GetHostAddress(),
                CountFavorite = 0,
                CountRead = 0,
                CountVote = 0,
                CountVoteAbstain = 0,
                CountVoteDown = 0,
                CountVoteUp = 0,
                CutText = "Read more...",
                IsPublished = true,
                IsPublishedDraft = false,
                IsPublishedIndex = false,
                IsRepost = false,
                IsTutorial = false,
                Rating = 0,
                SourceAuthor = null,
                SourceLink = null,
                TopicType = "topic",
                ForbidComment = false,
                EditDate = null
            };

            authorizationManager.EnsureAccess(principal, "AddNew", topic);

            using (var writeContext = this.writeContextFactory.Create())
            {
                writeContext.Set<Topic>().Add(topic);
                writeContext.SaveChanges();

                foreach (var tag in command.Tags.Split(','))
                {
                    writeContext.Set<TopicTag>().Add(new TopicTag { TagText = tag.Trim(), TopicId = topic.Id });
                }

                writeContext.Set<TopicContent>().Add(
                    new TopicContent
                        {
                            Topic = topic,
                            TopicText = this.topicsTextTransform.Transform(command.Text),
                            TopicTextSource = command.Text,
                            TopicTextShort = this.topicsTextTransform.Transform(this.shortTextExtractor.Extract(command.Text)),
                            TopicExtra = "Read more..."
                        });

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

            this.eventBus.Raise(
                new TopicCreatedEvent
                    {
                        Title = command.Title,
                        TopicId = topic.Id,
                        Tags = command.Tags,
                        Text = command.Text,
                        AuthorId = principal.GetUserId().GetValueOrDefault()
                    });

            return topic.Id;
        }
    }
}