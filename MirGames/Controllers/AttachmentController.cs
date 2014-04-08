namespace MirGames.Controllers
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Attachments.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// The attachment controller.
    /// </summary>
    public class AttachmentController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public AttachmentController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult Index(int attachmentId)
        {
            var attachment = this.QueryProcessor.Process(new GetAttachmentInfoQuery { AttachmentId = attachmentId });

            this.Response.Cache.SetCacheability(HttpCacheability.Public);
            this.Response.Cache.SetMaxAge(TimeSpan.FromDays(7));
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(7));

            return this.File(attachment.FilePath, attachment.ContentType, attachment.IsImage ? null : attachment.FileName);
        }

        /// <inheritdoc />
        [HttpPost]
        [Authorize(Roles = "User")]
        [AntiForgery]
        public JsonResult Upload()
        {
            string fileName = Encoding.UTF8.GetString(Convert.FromBase64String(Request.Headers["X-File-Name"]));
            string entityType = Request.Headers["X-Entity-Type"];
            long fileSize = Convert.ToInt64(Request.Headers["X-File-Size"]);

            var configSection = (HttpRuntimeSection)ConfigurationManager.GetSection("system.web/httpRuntime");

            if (fileSize > configSection.MaxRequestLength * 1024L)
            {
                return Json(new { error = string.Format("Максимальный допустимый размер файла: {0} МБ", configSection.MaxRequestLength / 1024) });
            }

            Stream fileContent = Request.InputStream;

            string tempPath = Path.GetTempFileName();
            using (FileStream fileStream = System.IO.File.Create(tempPath))
            {
                fileContent.Seek(0, SeekOrigin.Begin);
                fileContent.CopyTo(fileStream);
            }

            var command = new CreateAttachmentCommand
                {
                    FileName = fileName,
                    FilePath = tempPath,
                    EntityType = entityType
                };

            try
            {
                var attachmentId = this.CommandProcessor.Execute(command);
                return Json(new { attachmentId });
            }
            catch (CommandProcessorException e)
            {
                return Json(new { error = e.InnerException.Message });
            }
        }
    }
}