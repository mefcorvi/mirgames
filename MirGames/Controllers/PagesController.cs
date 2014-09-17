// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PagesController.cs">
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
    /// The static pages controller.
    /// </summary>
    public partial class PagesController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagesController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public PagesController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public virtual ActionResult About()
        {
            return this.View();
        }

        /// <inheritdoc />
        public virtual ActionResult Rules()
        {
            return this.View();
        }

        /// <inheritdoc />
        public virtual ActionResult Help()
        {
            return this.View();
        }
    }
}