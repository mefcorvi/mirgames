namespace MirGames.Infrastructure.Commands
{
    using System;

    /// <summary>
    /// Marks the command or query as non traceable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DisableTracingAttribute : Attribute
    {
    }
}