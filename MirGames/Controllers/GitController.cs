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