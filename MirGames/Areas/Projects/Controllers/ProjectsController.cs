﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectsController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Areas.Projects.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using MirGames.Controllers;
    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The WIP controller.
    /// </summary>
    public partial class ProjectsController : AppController
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
        public virtual ActionResult Index(string tag = null, int page = 1)
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

            var tags = this.QueryProcessor.Process(new GetWipTagsQuery(), new PaginationSettings(0, 50));
            ViewBag.Tags = tags;
            ViewBag.Tag = tag;

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                projectsCount,
                p => this.Url.ActionCached(MVC.Projects.Projects.Index(tag, p)));

            this.ViewBag.PageData["tag"] = tag;

            return this.View(projects);
        }

        /// <inheritdoc />
        public virtual ActionResult RedirectToProject(int projectId)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    ProjectId = projectId
                });

            return this.RedirectToActionPermanent("Project", new { projectAlias = project.Alias });
        }

        /// <inheritdoc />
        public virtual ActionResult Project(string projectAlias)
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
                    EntityType = "project-gallery",
                    IsImage = true,
                    OrderingBy = AttachmentsOrderingType.Random
                },
                new PaginationSettings(0, 3));

            this.LoadStatistics(project);

            var topics = this.QueryProcessor.Process(new GetWipProjectTopicsQuery { Alias = projectAlias }, new PaginationSettings(0, 10));

            if (project.CanReadRepository)
            {
                var commits = this.QueryProcessor.Process(new GetWipProjectCommitsQuery { Alias = projectAlias }, new PaginationSettings(0, 15));
                this.ViewBag.Commits = commits;
            }

            this.ViewBag.Topics = topics;
            this.ViewBag.Images = images;
            
            this.ViewBag.BackUrl = this.GetBackUrl();
            this.ViewBag.SubSection = "Project";

            return this.View(project);
        }

        /// <inheritdoc />
        public virtual ActionResult Gallery(string projectAlias)
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
                    EntityType = "project-gallery",
                    IsImage = true
                },
                new PaginationSettings(0, 30));

            this.LoadStatistics(project);

            this.ViewBag.Images = images;
            this.ViewBag.BackUrl = this.GetBackUrl();
            this.ViewBag.SubSection = "Gallery";

            this.PageData["projectAlias"] = project.Alias;

            return this.View(project);
        }

        /// <inheritdoc />
        public virtual ActionResult Archive(string projectAlias)
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
        public virtual ActionResult WorkItems(string projectAlias, string tag, WorkItemType? itemType)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            this.LoadStatistics(project);

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
        public virtual ActionResult WorkItemDialog()
        {
            /*var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            this.LoadStatistics(project);

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

            return this.View(project);            */
            return this.PartialView("_WorkItemDialog");
        }

        /// <inheritdoc />
        [Authorize(Roles = "User")]
        public virtual ActionResult New()
        {
            return this.View();
        }

        /// <inheritdoc />
        public virtual ActionResult GalleryItemDialog()
        {
            return this.View("_GalleryItemDialog");
        }

        /// <inheritdoc />
        [Authorize(Roles = "User")]
        public virtual ActionResult Settings(string projectAlias)
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            this.LoadStatistics(project);

            this.ViewBag.BackUrl = this.GetBackUrl();
            this.ViewBag.SubSection = "Settings";

            this.PageData["project"] = project;

            return this.View(project);
        }

        /// <inheritdoc />
        public virtual ActionResult Code(string projectAlias, string path = "/")
        {
            var project = this.QueryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            this.ViewBag.SubSection = "Repository";

            this.LoadStatistics(project);

            if (path.EndsWith("/"))
            {
                return this.RepositoryDirectory(path, project);
            }

            return this.RepositoryFile(path, project);
        }

        /// <inheritdoc />
        public virtual ActionResult AddWorkItemDialog()
        {
            return this.PartialView("_AddWorkItemDialog");
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "WIP";
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Loads the statistics.
        /// </summary>
        /// <param name="project">The project.</param>
        private void LoadStatistics(WipProjectViewModel project)
        {
            var statistics = this.QueryProcessor.Process(new GetProjectWorkItemStatisticsQuery
            {
                ProjectAlias = project.Alias
            });

            this.ViewBag.Statistics = statistics;
        }

        /// <summary>
        /// Repositories the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="project">The project.</param>
        /// <returns>The action result.</returns>
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
                       : this.Url.ActionCached(MVC.Projects.Projects.Index());
        }
    }
}