﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DashboardController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The dashboard controller.
    /// </summary>
    public class DashboardController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public DashboardController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult Index()
        {
            var model = new DashboardViewModel();

            var topicsQuery = new GetTopicsQuery { IsPublished = true };
            model.Topics = this.QueryProcessor.Process(topicsQuery, new PaginationSettings(0, 10));

            var paginationSettings = new PaginationSettings(0, 40);
            var froumTopicsQuery = new GetForumTopicsQuery();
            model.ForumTopics = this.QueryProcessor.Process(froumTopicsQuery, paginationSettings);

            var topicsPagination = new Dictionary<int, PaginationViewModel>();
            foreach (var topic in model.ForumTopics)
            {
                int topicId = topic.TopicId;
                topicsPagination[topicId] =
                    new PaginationViewModel(
                        new PaginationSettings(PaginationSettings.GetItemPage(topic.PostsCount, 20), 20),
                        topic.PostsCount,
                        p => this.GetForumTopicPageUrl(p, topicId))
                    {
                        ShowPrevNextNavigation = false,
                        HightlightCurrentPage = false
                    };
            }

            ViewBag.TopicsPagination = topicsPagination;

            model.Projects = this.QueryProcessor.Process(new GetWipProjectsQuery(), new PaginationSettings(0, 4));

            return View(model);
        }

        /// <summary>
        /// Gets the topic page URL.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="topicId">The topic unique identifier.</param>
        /// <returns>The topic page URL.</returns>
        private string GetForumTopicPageUrl(int page, int topicId)
        {
            return this.Url.Action(
                "Topic",
                "Forum",
                new { page, topicId }) + "#posts";
        }


        public class DashboardViewModel
        {
            /// <summary>
            /// Gets or sets the topics.
            /// </summary>
            public IEnumerable<TopicsListItem> Topics { get; set; }

            /// <summary>
            /// Gets or sets the forum topics.
            /// </summary>
            public IEnumerable<ForumTopicsListItemViewModel> ForumTopics { get; set; }

            /// <summary>
            /// Gets or sets the projects.
            /// </summary>
            public IEnumerable<WipProjectViewModel> Projects { get; set; }
        }
    }
}