// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectsController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Areas.Projects.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Exception;
    using MirGames.Infrastructure.Utilities;

    /// <summary>
    /// The WIP controller.
    /// </summary>
    public partial class PagesController : Controller
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The content type provider.
        /// </summary>
        private readonly IContentTypeProvider contentTypeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="contentTypeProvider">The content type provider.</param>
        public PagesController(IQueryProcessor queryProcessor, IContentTypeProvider contentTypeProvider)
        {
            this.queryProcessor = queryProcessor;
            this.contentTypeProvider = contentTypeProvider;
        }

        /// <inheritdoc />
        [OutputCache(VaryByParam = "url;path", Duration = 60)]
        public virtual ActionResult Index(string url, string path = "/")
        {
            var urlParts = (url ?? string.Empty).Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length == 0)
            {
                return this.HttpNotFound();
            }

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
            }

            if (path.EndsWith("/"))
            {
                path += "index.html";
            }

            var projectAlias = urlParts[0];

            var project = this.queryProcessor.Process(
                new GetWipProjectQuery
                {
                    Alias = projectAlias
                });

            return this.RepositoryFile(path, project);
        }

        /// <summary>
        /// Repositories the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="project">The project.</param>
        /// <returns>The action result.</returns>
        private ActionResult RepositoryFile(string path, WipProjectViewModel project)
        {
            try
            {
                var projectFile = this.queryProcessor.Process(new GetWipProjectFileQuery
                {
                    Alias = project.Alias,
                    FilePath = path
                });
                
                string contentType = this.contentTypeProvider.GetContentType(Path.GetExtension(projectFile.FileName));

                return this.File(projectFile.Content, contentType);
            }
            catch (QueryProcessingFailedException ex)
            {
                if (ex.InnerException is ItemNotFoundException)
                {
                    return this.HttpNotFound();
                }

                throw ex.InnerException;
            }
        }
    }
}