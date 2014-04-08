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