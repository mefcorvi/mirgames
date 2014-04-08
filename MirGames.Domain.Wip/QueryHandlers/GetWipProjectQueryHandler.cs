namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Queries;

    /// <summary>
    /// Returns the WIP project.
    /// </summary>
    internal sealed class GetWipProjectQueryHandler : SingleItemQueryHandler<GetWipProjectQuery, WipProjectViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="settings">The settings.</param>
        public GetWipProjectQueryHandler(IQueryProcessor queryProcessor, ISettings settings)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(settings != null);

            this.queryProcessor = queryProcessor;
            this.settings = settings;
        }

        /// <inheritdoc />
        public override WipProjectViewModel Execute(IReadContext readContext, GetWipProjectQuery query, ClaimsPrincipal principal)
        {
            var project = readContext
                .Query<Project>()
                .SingleOrDefault(p => p.ProjectId == query.ProjectId || p.Alias == query.Alias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.ProjectId);
            }

            var projectViewModel = new WipProjectViewModel
                {
                    CreationDate = project.CreationDate,
                    Author = new AuthorViewModel
                        {
                            Id = project.AuthorId
                        },
                    Description = project.Description,
                    Alias = project.Alias,
                    FollowersCount = project.FollowersCount,
                    ProjectId = project.ProjectId,
                    Title = project.Title,
                    UpdatedDate = project.UpdatedDate,
                    Version = project.Version,
                    Votes = project.Votes,
                    VotesCount = project.VotesCount,
                    Tags = project.TagsList.Split(',')
                };

            var attachment = this.queryProcessor.Process(new GetAttachmentsQuery
            {
                EntityId = project.ProjectId,
                EntityType = "project-logo",
                IsImage = true
            }).FirstOrDefault();

            if (attachment != null)
            {
                projectViewModel.LogoUrl = attachment.AttachmentUrl;
            }

            if (project.RepositoryId.HasValue && project.RepositoryType.EqualsIgnoreCase("git"))
            {
                var repository = this.queryProcessor.Process(new GetRepositoryQuery
                {
                    RepositoryId = project.RepositoryId.GetValueOrDefault()
                });

                projectViewModel.RepositoryUrl =
                    string.Format(this.settings.GetValue<string>("Repositories.Git.Url"), repository.RepositoryName);
            }

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = new[] { projectViewModel.Author }
                    });

            return projectViewModel;
        }
    }
}
