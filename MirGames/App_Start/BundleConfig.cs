﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="BundleConfig.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
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
                    "~/Scripts/Libs/qbaka.js",
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
                    "~/Scripts/Account/*.js",
                    "~/Scripts/UI/*.js",
                    "~/Scripts/chat/*.js",
                    "~/Scripts/topics/*.js",
                    "~/Scripts/Tools/*.js",
                    "~/Scripts/users/*.js",
                    "~/Scripts/Forum/*.js",
                    "~/Scripts/projects/*.js",
                    "~/Scripts/Attachment/*.js",
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
                "~/Content/nanoscroller.less",
                "~/Content/avatars.less",
                "~/Content/common.less",
                "~/Content/text.less",
                "~/Content/wmd.less",
                "~/Content/dialogs.less",
                "~/Content/forms.less",
                "~/Content/quick-date.less",
                "~/Content/ui/*.less",
                "~/Content/account/*.less",
                "~/Content/chat/*.less",
                "~/Content/topics/*.less",
                "~/Content/users/*.less",
                "~/Content/forum/*.less",
                "~/Content/tools/*.less",
                "~/Content/dashboard/*.less",
                "~/Content/projects/*.less",
                "~/Content/font-awesome/font-awesome.less");

            bundles.Add(cssBundle);
        }
    }
}