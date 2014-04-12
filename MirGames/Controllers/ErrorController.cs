// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ErrorController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Web.Mvc;

    using MirGames.Infrastructure;

    /// <summary>
    /// The forum controller.
    /// </summary>
    public class ErrorController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ErrorController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult General()
        {
            Response.StatusCode = 500;
            return View();
        }

        /// <inheritdoc />
        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        /// <inheritdoc />
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
