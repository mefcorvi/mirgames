// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="BundleConfig.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames
{
    using System.Web.Optimization;

    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// The bundle config.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            IBundleTransform jstransformer = new JsTransformer();
            IBundleTransform cssTransformer = new CssTransformer();

            var scripts = new[]
                {
                    Links.Scripts.Libs.qbaka_js,
                    "~/Scripts/Libs/markdown/AutoLinkTransform.js",
                    "~/Scripts/Libs/markdown/MarkdownDeep.js",
                    "~/Scripts/Libs/markdown/MarkdownDeepEditor.js",
                    "~/Scripts/Libs/markdown/MarkdownDeepEditorUI.js",
                    "~/Scripts/Libs/jquery-{version}.js",
                    "~/Scripts/Libs/jquery.signalR-{version}.js",
                    "~/Scripts/Libs/jquery.cookie.js",
                    "~/Scripts/Libs/jquery.nanoscroller.js",
                    "~/Scripts/Libs/jquery.naturalWidth.js",
                    "~/Scripts/Libs/jquery.hashchange.js",
                    "~/Scripts/Libs/jquery.autosize.js",
                    "~/Scripts/Libs/MD5.js",
                    "~/Scripts/Libs/angular.js",
                    "~/Scripts/Libs/angular-recaptcha.js",
                    "~/Scripts/Libs/ng-quick-date.js",
                    "~/Scripts/Libs/ui-bootstrap-custom-{version}.js",
                    "~/Scripts/Libs/eventEmitter.js",
                    "~/Scripts/Libs/linq.js",
                    "~/Scripts/Libs/tinycon.js",
                    "~/Scripts/Libs/ion.sound.js",
                    "~/Scripts/Libs/moment.js",
                    "~/Scripts/Core/Base64.js",
                    "~/Scripts/Core/Utils.js",
                    "~/Scripts/Core/Config.js",
                    "~/Scripts/TypeLiteEnums.js",
                    "~/Scripts/Settings.js",
                    "~/Scripts/Commands.js",
                    "~/Scripts/BasePage.js",
                    "~/Scripts/Core/Application.js",
                    "~/Scripts/Core/CurrentUser.js",
                    "~/Scripts/Core/ApiService.js",
                    "~/Scripts/Core/Service.js",
                    "~/Scripts/Core/EventBus.js",
                    "~/Scripts/Core/CommandBus.js",
                    "~/Scripts/Core/SocketService.js",
                    Links.Scripts.Account.Url("*.js"),
                    Links.Scripts.UI.Url("*.js"),
                    Links.Areas.Chat.Scripts.Url("*.js"),
                    Links.Areas.Topics.Scripts.Url("*.js"),
                    Links.Scripts.Tools.Url("*.js"),
                    Links.Scripts.Users.Url("*.js"),
                    Links.Areas.Forum.Scripts.Url("*.js"),
                    Links.Scripts.projects.Url("*.js"),
                    Links.Scripts.Attachment.Url("*.js"),
                    "~/Scripts/MirGames.js",
                    "~/Scripts/ActivationNotificationController.js",
                    "~/Scripts/NavigationController.js",
                    "~/Scripts/OnlineUsersController.js",
                    "~/Scripts/UserNotificationController.js",
                    "~/Scripts/RequestNotificationController.js",
                    "~/Scripts/SocketNotificationController.js"
                };

            var bundle = new Bundle("~/Scripts/js", jstransformer);
            bundle.Include(scripts);

            bundles.Add(bundle);

            var cssBundle = new Bundle("~/Content/css", cssTransformer);
            cssBundle.Include(
                Links.Content.nanoscroller_less,
                Links.Content.avatars_less,
                Links.Content.common_less,
                Links.Content.text_less,
                Links.Content.wmd_less,
                Links.Content.dialogs_less,
                Links.Content.forms_less,
                Links.Content.quick_date_less,
                Links.Content.ui.Url("*.less"),
                Links.Areas.Chat.Content.Url("*.less"),
                Links.Areas.Topics.Content.Url("*.less"),
                Links.Content.users.Url("*.less"),
                Links.Areas.Forum.Content.Url("*.less"),
                Links.Content.tools.Url("*.less"),
                Links.Content.dashboard.Url("*.less"),
                Links.Content.projects.Url("*.less"),
                Links.Content.font_awesome.font_awesome_less);

            bundles.Add(cssBundle);
        }
    }
}