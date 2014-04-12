// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetTopicForEditQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;

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
        /// Initializes a new instance of the <see cref="GetTopicForEditQueryHandler"/> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetTopicForEditQueryHandler(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override TopicForEditViewModel Execute(IReadContext readContext, GetTopicForEditQuery query, ClaimsPrincipal principal)
        {
            Topic topic = readContext
                .Query<Topic>()
                .Include(t => t.Content)
                .SingleOrDefault(t => t.Id == query.TopicId);

            if (topic == null)
            {
                return null;
            }

            if (!this.authorizationManager.CheckAccess(principal, "Edit", topic))
            {
                throw new SecurityException("User could not edit the specified topic.");
            }

            return new TopicForEditViewModel
                {
                    Id = topic.Id,
                    Text = topic.Content.TopicTextSource,
                    Title = topic.TopicTitle,
                    Tags = topic.TagsList
                };
        }
    }
}