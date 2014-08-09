namespace MirGames.Areas.Projects
{
    using System.Web.Mvc;

    using MirGames.Domain.Wip.ViewModels;

    public class ProjectsAreaRegistration : AreaRegistration 
    {
        /// <inheritdoc />
        public override string AreaName 
        {
            get { return "Projects"; }
        }

        /// <inheritdoc />
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WipRedirectToProject",
                "project/redirect/{projectId}",
                new { controller = "Projects", action = "RedirectToProject" },
                new { projectId = @"\d+" });

            context.MapRoute(
                "HistoryItem",
                "project/{projectAlias}/code/{*path}",
                new { controller = "Projects", action = "Code", path = "/" });

            context.MapRouteLowercase(
                "WipProjectIndex",
                "project/{projectAlias}",
                new { controller = "Projects", action = "Project" });

            context.MapRouteLowercase(
                "WipProjectArchive",
                "projects/{projectAlias}.zip",
                new { controller = "Projects", action = "Archive" });

            context.MapRouteLowercase(
                "WipProjectSettings",
                "project/{projectAlias}/settings",
                new { controller = "Projects", action = "Settings" });

            context.MapRouteLowercase(
                "WipProjectWorkItemsBugs",
                "project/{projectAlias}/workitems/bugs",
                new { controller = "Projects", action = "WorkItems", itemType = WorkItemType.Bug });

            context.MapRouteLowercase(
                "WipProjectWorkItemsFeatures",
                "project/{projectAlias}/workitems/features",
                new { controller = "Projects", action = "WorkItems", itemType = WorkItemType.Feature });

            context.MapRouteLowercase(
                "WipProjectWorkItemsTasks",
                "project/{projectAlias}/workitems/tasks",
                new { controller = "Projects", action = "WorkItems", itemType = WorkItemType.Task });

            context.MapRouteLowercase(
                "WipProjectWorkItems",
                "project/{projectAlias}/workitems",
                new { controller = "Projects", action = "WorkItems", itemType = (WorkItemType?)null });

            context.MapRouteLowercase(
                "WipProjectWorkItem",
                "project/{projectAlias}/workitems/{workItemId}",
                new { controller = "Projects", action = "WorkItem" });

            context.MapRouteLowercase(
                "WipProjectCode",
                "project/{projectAlias}/code",
                new { controller = "Projects", action = "Code" });

            context.MapRouteLowercase(
                "Projects_default",
                "projects/{action}/{id}",
                new { controller = "Projects", action = "Index", id = UrlParameter.Optional });
        }
    }
}