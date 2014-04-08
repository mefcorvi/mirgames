namespace MirGames.Infrastructure.Commands
{
    using System.Diagnostics.CodeAnalysis;

    using Newtonsoft.Json;

    /// <summary>
    /// The synchronous command.
    /// </summary>
    [JsonConverter(typeof(CommandsConverter))]
    public abstract class Command
    {
    }

    /// <summary>
    /// The synchronous command.
    /// </summary>
    /// <typeparam name="T">Type of result.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class Command<T> : Command
    {
    }
}
