namespace MirGames
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Web.Mvc;

    using MirGames.Domain.Users.ViewModels;

    /// <summary>
    /// The base class of application page.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class BasePage<TModel> : WebViewPage<TModel>
    {
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