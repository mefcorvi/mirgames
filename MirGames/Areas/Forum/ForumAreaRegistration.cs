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
                "ForumUnreadItems",
                "forum/unread",
                new { controller = "Forum", action = "Topics", onlyUnread = true });

            context.MapRoute(
                "ForumTopicItems",
                "forum/{forumAlias}",
                new { controller = "Forum", action = "Topics", onlyUnread = false, page = 1 });

            context.MapRoute(
                "ForumTopicsListWithPage",
                "forum/{forumAlias}/page{page}",
                new { controller = "Forum", action = "Topics", onlyUnread = false },
                new { page = @"\d+" });

            context.MapRoute(
                "ForumTopicItemFirstPage",
                "forum/{forumAlias}/{topicId}",
                new { controller = "Forum", action = "Topic", page = 1 },
                new { topicId = @"\d+", page = @"\d+" });

            context.MapRoute(
                "ForumTopicItem",
                "forum/{forumAlias}/{topicId}/{page}",
                new { controller = "Forum", action = "Topic" },
                new { topicId = @"\d+", page = @"\d+" });

            context.MapRoute(
                "ForumAllItems",
                "forum",
                new { controller = "Forum", action = "Index" });

            context.MapRouteLowercase(
                "Forum_default",
                "forum/{action}/{id}",
                new { controller = "Forum", action = "Index", id = UrlParameter.Optional });
        }
    }
}