// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ProjectLinkRegistrar.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Areas.Projects
{
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using MirGames.Infrastructure;

    /// <summary>
    /// Registrar of projects link.
    /// </summary>
    internal sealed class ProjectLinkRegistrar : IEntityLinkRegistrar
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            Contract.Requires(entityType != null);
            return entityType.EqualsIgnoreCase("Project");
        }

        /// <inheritdoc />
        public string GetLink(int? entityId, string entityType)
        {
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));
            return urlHelper.Action(MVC.Projects.Projects.RedirectToProject(entityId.GetValueOrDefault()));
        }
    }
}