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
                new { controller = "Forum", action = "Unread" });

            context.MapRoute(
                "Rss",
                "forum/rss",
                new { controller = "Forum", action = "Rss" });

            context.MapRoute(
                "EditPostDialog",
                "forum/editPostDialog",
                new { controller = "Forum", action = "EditPostDialog" });

            context.MapRoute(
                "DeletePostDialog",
                "forum/deletePostDialog",
                new { controller = "Forum", action = "DeletePostDialog" });

            context.MapRoute(
                "DeleteTopicDialog",
                "forum/deleteTopicDialog",
                new { controller = "Forum", action = "DeleteTopicDialog" });

            context.MapRoute(
                "ForumNewItems",
                "forum/{forumAlias}/new",
                new { controller = "Forum", action = "New" });

            context.MapRoute(
                "ForumDeleteDialogs",
                "forum/deletepostdialog",
                new { controller = "Forum", action = "DeletePostDialog" });

            context.MapRoute(
                "LegacyForumTopicItem",
                "forum/{topicId}/{page}",
                new { controller = "Forum", action = "Topic", page = 1 },
                new { topicId = @"\d+", page = @"\d*", forumAlias = string.Empty });

            context.MapRoute(
                "ForumTopicItems",
                "forum/{forumAlias}",
                new { controller = "Forum", action = "Topics", page = 1 });

            context.MapRoute(
                "ForumTopicsListWithPage",
                "forum/{forumAlias}/page{page}",
                new { controller = "Forum", action = "Topics" },
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