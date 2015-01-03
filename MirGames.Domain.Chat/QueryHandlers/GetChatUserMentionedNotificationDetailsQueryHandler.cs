// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetChatUserMentionedNotificationDetailsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Notifications;
    using MirGames.Domain.Notifications.Extensions;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the <see cref="GetChatUserMentionedNotificationDetailsQueryHandler"/> query.
    /// </summary>
    internal sealed class GetChatUserMentionedNotificationDetailsQueryHandler : QueryHandler<GetNotificationDetailsQuery, NotificationDetailsViewModel>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChatUserMentionedNotificationDetailsQueryHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textProcessor">The text processor.</param>
        public GetChatUserMentionedNotificationDetailsQueryHandler(
            IReadContextFactory readContextFactory,
            IQueryProcessor queryProcessor,
            ITextProcessor textProcessor)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(textProcessor != null);

            this.readContextFactory = readContextFactory;
            this.queryProcessor = queryProcessor;
            this.textProcessor = textProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetNotificationDetailsQuery query, ClaimsPrincipal principal)
        {
            return query.Notifications.Count(n => n is ChatMentionNotification);
        }

        /// <inheritdoc />
        protected override IEnumerable<NotificationDetailsViewModel> Execute(
            GetNotificationDetailsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var notifications = query.Notifications.OfType<ChatMentionNotification>().ToArray();
            var affectedMessages = notifications.Select(n => n.MessageId).ToArray();

            if (!affectedMessages.Any())
            {
                return Enumerable.Empty<NotificationDetailsViewModel>();
            }

            List<ChatMentionNotificationDetailsViewModel> messages;
            using (var readContext = this.readContextFactory.Create())
            {
                messages = readContext
                    .Query<ChatMessage>()
                    .Where(message => affectedMessages.Contains(message.MessageId))
                    .Select(message => new ChatMentionNotificationDetailsViewModel
                    {
                        MessageId = message.MessageId,
                        Author = new AuthorViewModel { Id = message.AuthorId, Login = message.AuthorLogin },
                        MessageText = message.Message
                    })
                    .ToList();
            }

            messages.ForEach(m =>
            {
                m.MessageText = this.textProcessor.GetHtml(m.MessageText);
            });

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = messages.Select(p => p.Author).ToArray()
            });

            return messages.CopyBaseNotificationData(notifications, model => model.MessageId, notification => notification.MessageId).ToList();
        }
    }
}
