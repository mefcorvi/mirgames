// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicsAreaRegistration.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Areas.Topics
{
    using System.Web.Mvc;

    public class TopicsAreaRegistration : AreaRegistration 
    {
        /// <inheritdoc />
        public override string AreaName 
        {
            get { return "Topics"; }
        }

        /// <inheritdoc />
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TopicItem",
                "topics/{topicId}",
                new { controller = "Topics", action = "Topic" },
                new { topicId = @"\d+" });

            context.MapRoute(
                "TopicListItem",
                "topics/listitem/{topicId}",
                new { controller = "Topics", action = "TopicListItem" },
                new { topicId = @"\d+" });

            context.MapRoute(
                "Tutorials",
                "topics/tutorials",
                new { controller = "Topics", action = "Tutorials", page = 1 });

            context.MapRoute(
                "TutorialsPaged",
                "topics/tutorials/page{page}",
                new { controller = "Topics", action = "Tutorials" },
                new { page = @"\d+" });

            context.MapRoute(
                "TopicsList",
                "topics",
                new { controller = "Topics", action = "Index", onlyTutorial = false, onlyUnread = false, page = 1 });

            context.MapRoute(
                "EditTopicItem",
                "topics/Edit/{topicId}",
                new { controller = "Topics", action = "Edit" },
                new { topicId = @"\d+" });

            context.MapRoute(
                "UnreadTopicsList",
                "topics/unread",
                new { controller = "Topics", action = "Index", onlyUnread = true, page = 1 });

            context.MapRoute(
                "TopicsListWithPage",
                "topics/page{page}",
                new { controller = "Topics", action = "Index", onlyUnread = false },
                new { page = @"\d+" });

            context.MapRoute(
                "Topics_default",
                "topics/{action}/{id}",
                new { controller = "Topics", action = "Index", id = UrlParameter.Optional });
        }
    }
}