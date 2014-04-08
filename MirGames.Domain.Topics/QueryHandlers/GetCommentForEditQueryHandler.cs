namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    internal sealed class GetCommentForEditQueryHandler : SingleItemQueryHandler<GetCommentForEditQuery, CommentForEditViewModel>
    {
        /// <inheritdoc />
        public override CommentForEditViewModel Execute(IReadContext readContext, GetCommentForEditQuery query, ClaimsPrincipal principal)
        {
            return readContext
                .Query<Comment>()
                .Where(c => c.CommentId == query.CommentId)
                .Select(c => new CommentForEditViewModel
                    {
                        Id = c.CommentId,
                        SourceText = c.SourceText
                    })
                .FirstOrDefault();
        }
    }
}