namespace MirGames.Areas.Forum
{
    using System.Web.Mvc;

    public class ForumAreaRegistration : AreaRegistration 
    {
        /// <inheritdoc />
        public override string AreaName 
        {
            get { return "Forum"; }
        }

        /// <inheritdoc />
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ForumTopicItemFirstPage",
                "forum/{topicId}",
                new { controller = "Forum", action = "Topic", page = 1 },
                new { topicId = @"\d+" });

            context.MapRoute(
                "ForumTopicItem",
                "forum/{topicId}/{page}",
                new { controller = "Forum", action = "Topic" },
                new { topicId = @"\d+", page = @"\d+" });

            context.MapRoute(
                "ForumAllItems",
                "forum",
                new { controller = "Forum", action = "Index", onlyUnread = false, page = 1 });

            context.MapRoute(
                "ForumUnreadItems",
                "forum/unread",
                new { controller = "Forum", action = "Index", onlyUnread = true, page = 1 });

            context.MapRoute(
                "ForumTopicsListWithPage",
                "forum/page{page}",
                new { controller = "Forum", action = "Index" },
                new { page = @"\d+" });

            context.MapRouteLowercase(
                "Forum_default",
                "forum/{action}/{id}",
                new { controller = "Forum", action = "Index", id = UrlParameter.Optional });
        }
    }
}