namespace MirGames.Controllers
{
    using System.IO;
    using System.Web.Mvc;

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

            var topics = this.QueryProcessor.Process(new GetWipProjectTopicsQuery { Alias = projectAlias }, new PaginationSettings(0, 10));
            var commits = this.QueryProcessor.Process(new GetWipProjectCommitsQuery { Alias = projectAlias });

            this.ViewBag.Topics = topics;
            this.ViewBag.Images = images;
            this.ViewBag.Commits = commits;
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
        [Authorize(Roles = "User")]
        public ActionResult New()
        {
            return this.View();
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