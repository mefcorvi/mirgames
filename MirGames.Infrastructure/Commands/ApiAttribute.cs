namespace MirGames.Infrastructure.Commands
{
    using System;

    /// <summary>
    /// Marks the command or query as API entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiAttribute : Attribute
    {
    }
}