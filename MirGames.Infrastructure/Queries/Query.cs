namespace MirGames.Infrastructure.Queries
{
    using System.Diagnostics.CodeAnalysis;

    using Newtonsoft.Json;

    /// <summary>
    /// Abstraction of query.
    /// </summary>
    [JsonConverter(typeof(QueriesConverter))]
    public abstract class Query
    {
    }

    /// <summary>
    /// Abstraction of query.
    /// </summary>
    /// <typeparam name="TResult">Type of entity.</typeparam>
    // ReSharper disable UnusedTypeParameter
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class Query<TResult> : Query
    {
    }
}