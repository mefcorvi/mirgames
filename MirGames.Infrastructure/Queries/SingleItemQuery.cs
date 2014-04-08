namespace MirGames.Infrastructure.Queries
{
    /// <summary>
    /// Abstraction of query which returns only one item.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class SingleItemQuery<TResult> : Query<TResult>
    {
    }
}