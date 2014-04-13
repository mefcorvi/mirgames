namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    [Api]
    public sealed class GetProjectWorkItemCommentsQuery : Query<ProjectWorkItemCommentViewModel>
    {
        /// <summary>
        /// Gets or sets the work item identifier.
        /// </summary>
        public int WorkItemId { get; set; }
    }
}