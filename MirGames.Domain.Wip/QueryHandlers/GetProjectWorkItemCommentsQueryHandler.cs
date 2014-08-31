namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetProjectWorkItemCommentsQueryHandler : QueryHandler<GetProjectWorkItemCommentsQuery, ProjectWorkItemCommentViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectWorkItemCommentsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetProjectWorkItemCommentsQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        protected override int GetItemsCount(IReadContext readContext, GetProjectWorkItemCommentsQuery query, ClaimsPrincipal principal)
        {
            return this.GetComments(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<ProjectWorkItemCommentViewModel> Execute(
            IReadContext readContext,
            GetProjectWorkItemCommentsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var comments = this.ApplyPagination(this.GetComments(readContext, query), pagination).ToList();

            var commentViewModels = comments
                .Select(c => new ProjectWorkItemCommentViewModel
                {
                    CommentId = c.CommentId,
                    Date = c.Date,
                    Text = c.Text,
                    UpdatedDate = c.UpdatedDate,
                    Author = new AuthorViewModel
                    {
                        Id = c.UserId,
                        Login = c.UserLogin
                    },
                    WorkItemId = c.WorkItemId
                })
                .ToList();

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = commentViewModels.Select(c => c.Author)
            });

            return commentViewModels;
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>Set of project work items.</returns>
        private IQueryable<ProjectWorkItemComment> GetComments(IReadContext readContext, GetProjectWorkItemCommentsQuery query)
        {
            return readContext.Query<ProjectWorkItemComment>().Where(p => p.WorkItemId == query.WorkItemId).OrderBy(comment => comment.Date);
        }
    }
}