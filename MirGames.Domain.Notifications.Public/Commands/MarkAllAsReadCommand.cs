namespace MirGames.Domain.Notifications.Commands
{
    using MirGames.Infrastructure.Commands;

    [Api("Помечает все нотификации текущего юзера, как прочитанные")]
    [Authorize(Roles = "User")]
    public sealed class MarkAllAsReadCommand : Command
    {
    }
}
