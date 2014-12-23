// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetTopicForEditQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the GetTopicQuery.
    /// </summary>
    internal sealed class GetTopicForEditQueryHandler : SingleItemQueryHandler<GetTopicForEditQuery, TopicForEditViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicForEditQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetTopicForEditQueryHandler(IAuthorizationManager authorizationManager, IReadContextFactory readContextFactory)
        {
            Contract.Requires(authorizationManager != null);
            Contract.Requires(readContextFactory != null);

            this.authorizationManager = authorizationManager;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override TopicForEditViewModel Execute(GetTopicForEditQuery query, ClaimsPrincipal principal)
        {
            Topic topic;
            using (var readContext = this.readContextFactory.Create())
            {
                topic = readContext
                    .Query<Topic>()
                    .Include(t => t.Content)
                    .SingleOrDefault(t => t.Id == query.TopicId);
            }

            if (topic == null)
            {
                return null;
            }

            if (!this.authorizationManager.CheckAccess(principal, "Edit", "Topic", topic.Id))
            {
                throw new SecurityException("User could not edit the specified topic.");
            }

            return new TopicForEditViewModel
                {
                    Id = topic.Id,
                    Text = topic.Content.TopicTextSource,
                    Title = topic.TopicTitle,
                    Tags = topic.TagsList,
                    IsMicroTopic = topic.IsMicroTopic
                };
        }
    }
}