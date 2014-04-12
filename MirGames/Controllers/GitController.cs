// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GitController.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Web;
    using System.Web.Mvc;

    using MirGames.Services.Git.Public.Services;

    public class GitController : Controller
    {
        /// <summary>
        /// The git service.
        /// </summary>
        private readonly IGitService gitService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitController"/> class.
        /// </summary>
        /// <param name="gitService">The git service.</param>
        public GitController(IGitService gitService)
        {
            this.gitService = gitService;
        }

        public ActionResult GetInfoRefs(string project, string service)
        {
            this.NoCache(string.Format("application/x-{0}-advertisement", service));
            this.Response.StatusCode = 200;

            try
            {
                this.gitService.GetInfoRefs(project, service, GetInputStream(this.Request), this.Response.OutputStream);
            }
            catch (UnauthorizedAccessException)
            {
                return this.UnauthorizedResult();
            }

            return new ContentResult();
        }

        [HttpPost]
        public ActionResult UploadPack(string project)
        {
            this.NoCache("application/x-git-upload-pack-result");

            try
            {
                this.gitService.UploadPack(project, GetInputStream(this.Request), this.Response.OutputStream);
            }
            catch (UnauthorizedAccessException)
            {
                return this.UnauthorizedResult();
            }

            return new ContentResult();
        }

        [HttpPost]
        public ActionResult ReceivePack(string project)
        {
            this.NoCache("application/x-git-receive-pack-result");

            try
            {
                this.gitService.ReceivePack(project, GetInputStream(this.Request), this.Response.OutputStream);
            }
            catch (UnauthorizedAccessException)
            {
                return this.UnauthorizedResult();
            }

            return new ContentResult();
        }

        private static Stream GetInputStream(HttpRequestBase request)
        {
            return request.Headers["Content-Encoding"] == "gzip"
                ? new GZipStream(request.InputStream, CompressionMode.Decompress)
                : request.InputStream;
        }

        private void NoCache(string contentType)
        {
            this.Response.AddHeader("Expires", "Fri, 01 Jan 1980 00:00:00 GMT");
            this.Response.AddHeader("Pragma", "no-cache");
            this.Response.AddHeader("Cache-Control", "no-cache, max-age=0, must-revalidate");

            this.Response.BufferOutput = false;
            this.Response.Charset = string.Empty;
            this.Response.ContentType = contentType;
        }

        private ActionResult UnauthorizedResult()
        {
            Response.Clear();
            Response.AddHeader("WWW-Authenticate", "Basic realm=\"Secure Area\"");
            Response.SuppressFormsAuthenticationRedirect = true;

            return new HttpStatusCodeResult(401);
        }
    }
}