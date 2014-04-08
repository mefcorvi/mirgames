namespace MirGames.Domain.Wip.Queries
{
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    [Api]
    public class GetIsProjectNameUniqueQuery : SingleItemQuery<bool>
    {
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }
    }
}