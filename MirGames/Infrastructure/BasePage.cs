// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="BasePage.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Web.Mvc;

    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;

    /// <summary>
    /// The base class of application page.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class BasePage<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Gets the entity link provider.
        /// </summary>
        public IEntityLinkProvider EntityLinkProvider
        {
            get { return DependencyResolver.Current.GetService<IEntityLinkProvider>(); }
        }

        /// <summary>
        /// Gets or sets the page script controller.
        /// </summary>
        public string PageScriptController
        {
            get { return this.ViewBag.ScriptController; }
            set { this.ViewBag.ScriptController = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return this.ViewBag.Title; }
            set { this.ViewBag.Title = value; }
        }

        /// <summary>
        /// Gets the recapcha public key.
        /// </summary>
        public string RecapchaPublicKey
        {
            get { return DependencyResolver.Current.GetService<IRecaptchaSettings>().GetPublicKey(); }
        }

        /// <summary>
        /// Gets the CSS classes.
        /// </summary>
        public ICollection<string> CssClasses
        {
            get { return this.InitializeViewBag().CssClasses; }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public new ClaimsPrincipal User
        {
            get { return (ClaimsPrincipal)base.User; }
        }

        /// <inheritdoc />
        public CurrentUserViewModel UserEntity
        {
            get { return ViewBag.User; }
        }

        /// <inheritdoc />
        public override void Execute()
        {
        }

        /// <summary>
        /// Sets the page CSS class.
        /// </summary>
        /// <param name="class">The class.</param>
        public void AddPageCssClass(string @class)
        {
            this.CssClasses.Add(@class);
        }

        /// <summary>
        /// Initializes the view bag.
        /// </summary>
        /// <returns>The view bag.</returns>
        private dynamic InitializeViewBag()
        {
            if (this.ViewBag.CssClasses == null)
            {
                this.ViewBag.CssClasses = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            }

            return this.ViewBag;
        }
    }
}