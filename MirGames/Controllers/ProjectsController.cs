// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectsController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The WIP controller.
    /// </summary>
    public sealed class ProjectsController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ProjectsController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult Index(string tag = null, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);

            var query = new GetWipProjectsQuery
                {
                    Tag = tag
                };

            var projects = this.QueryProcessor.Process(query, paginationSettings);
            var projectsCount = this.QueryProcessor.GetItemsCount(query);

            var tags = this.QueryProcessor.Process(new GetWipTagsQuery());
            ViewBag.Tags = tags;
            ViewBag.Tag = tag;

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                projectsCount,
                p => this.Url.Action("Project", "Projects", new { tag, page = p }));

            this.ViewBag.PageData["tag"] = tag;

            return View(projects);
        }

        /// <inheritdoc />
        public ActionResult Project(string projectAlias)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                    {
                        Alias = projectAlias
                    });

            var images = this.QueryProcessor.Process(
                new GetAttachmentsQuery
                {
                    EntityId = project.ProjectId,
                    EntityType = "project",
                    IsImage = true
                },
                new PaginationSettings(0, 5));

            var statistics = this.QueryProcessor.Process(new GetProjectWorkItemStatisticsQuery
            {
                ProjectAlias = project.Alias
            });

            var topics = this.QueryProcessor.Process(new GetWipProjectTopicsQuery { Alias = projectAlias }, new PaginationSettings(0, 10));

            var commits = this.QueryProcessor.Process(new GetWipProjectCommitsQuery { Alias = projectAlias }, new PaginationSettings(0, 15));
            this.ViewBag.Commits = commits;
            this.ViewBag.Topics = topics;
            this.ViewBag.Images = images;
            this.ViewBag.Statistics = statistics;
            
            this.ViewBag.BackUrl = this.GetBackUrl();
            this.ViewBag.SubSection = "Project";

            return View(project);
        }

        /// <inheritdoc />
        public ActionResult Archive(string projectAlias)
        {
            var stream = new MemoryStream();
            this.CommandProcessor.Execute(new ArchiveProjectRepositoryCommand
            {
                OutputStream = stream,
                ProjectAlias = projectAlias
            });

            stream.Position = 0;
            return this.File(stream, "application/zip", projectAlias + ".zip");
        }

        /// <inheritdoc />
        public ActionResult WorkItems(string projectAlias, string tag, WorkItemType? itemType)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            var workItems = this.QueryProcessor.Process(new GetProjectWorkItemsQuery
            {
                ProjectAlias = projectAlias,
                Tag = tag,
                WorkItemType = itemType
            });

            this.ViewBag.SubSection = "WorkItems";

            var availableItems = new List<WorkItemType>();

            if (project.CanCreateBug)
            {
                availableItems.Add(WorkItemType.Bug);
            }

            if (project.CanCreateTask)
            {
                availableItems.Add(WorkItemType.Task);
            }

            if (project.CanCreateFeature)
            {
                availableItems.Add(WorkItemType.Feature);
            }

            this.PageData["availableItemTypes"] = availableItems.Cast<int>().ToArray();
            this.PageData["projectAlias"] = project.Alias;
            this.PageData["workItems"] = workItems;
            this.PageData["filterByType"] = itemType;

            this.ViewBag.BackUrl = this.GetBackUrl();

            return this.View(project);
        }

        /// <inheritdoc />
        public ActionResult WorkItem(string projectAlias, int workItemId)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            var workItem = this.QueryProcessor.Process(new GetProjectWorkItemQuery
            {
                ProjectAlias = projectAlias,
                InternalId = workItemId
            });

            var comments = this.QueryProcessor.Process(new GetProjectWorkItemCommentsQuery
            {
                WorkItemId = workItem.WorkItemId
            });

            this.PageData["projectAlias"] = project.Alias;
            this.PageData["workItemId"] = workItem.WorkItemId;

            this.ViewBag.SubSection = "WorkItems";
            this.ViewBag.WorkItem = workItem;
            this.ViewBag.BackUrl = this.GetBackUrl();
            this.ViewBag.Comments = comments;

            return this.View(project);            
        }
        
        /// <inheritdoc />
        [Authorize(Roles = "User")]
        public ActionResult New()
        {
            return this.View();
        }

        /// <inheritdoc />
        [Authorize(Roles = "User")]
        public ActionResult Settings(string projectAlias)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            this.ViewBag.BackUrl = this.GetBackUrl();
            this.ViewBag.SubSection = "Settings";

            this.PageData["project"] = project;

            return View(project);
        }

        /// <inheritdoc />
        public ActionResult Code(string projectAlias, string path = "/")
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            this.ViewBag.SubSection = "Repository";

            if (path.EndsWith("/"))
            {
                return this.RepositoryDirectory(path, project);
            }

            return this.RepositoryFile(path, project);
        }

        /// <inheritdoc />
        public ActionResult AddWorkItemDialog()
        {
            return this.PartialView("_AddWorkItemDialog");
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "WIP";
            base.OnActionExecuting(filterContext);
        }

        private ActionResult RepositoryFile(string path, WipProjectViewModel project)
        {
            var projectFiles = this.QueryProcessor.Process(new GetWipProjectFilesQuery
            {
                Alias = project.Alias,
                RelativePath = Path.GetDirectoryName(path)
            });

            var projectFile = this.QueryProcessor.Process(new GetWipProjectFileQuery
            {
                Alias = project.Alias,
                FilePath = path
            });

            this.ViewBag.File = projectFile;
            this.ViewBag.Files = projectFiles;
            this.ViewBag.ParentFolder = Path.GetDirectoryName(path);
            this.ViewBag.BackUrl = this.GetBackUrl();

            return this.View("RepositoryFile", project);
        }

        private ActionResult RepositoryDirectory(string path, WipProjectViewModel project)
        {
            path = path == "/" ? string.Empty : path;

            var projectFiles = this.QueryProcessor.Process(new GetWipProjectFilesQuery
            {
                Alias = project.Alias,
                RelativePath = path
            });

            this.ViewBag.Files = projectFiles;
            this.ViewBag.ParentFolder = string.IsNullOrEmpty(path) ? null : Path.GetDirectoryName(path);
            this.ViewBag.BackUrl = this.GetBackUrl();

            return this.View("RepositoryDirectory", project);
        }

        private string GetBackUrl()
        {
            return this.HttpContext.Request.UrlReferrer != null
                   && this.HttpContext.Request.UrlReferrer.IsRouteMatch("Wip", "Index")
                       ? this.HttpContext.Request.UrlReferrer.ToString()
                       : this.Url.Action("Index", "Projects");
        }
    }
}