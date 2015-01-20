namespace MirGames.Areas.Chat
{
    using System.Web.Mvc;

    public class ChatAreaRegistration : AreaRegistration 
    {
        /// <inheritdoc />
        public override string AreaName 
        {
            get { return "Chat"; }
        }

        /// <inheritdoc />
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "ChatMessage",
                "chat/{messageId}",
                new { controller = "Chat", action = "Index" });

            context.MapRouteLowercase(
                "Chat_default",
                "chat/{action}/{id}",
                new { controller = "Chat", action = "Index", id = UrlParameter.Optional });
        }
    }
}