// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AttachmentController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using ImageResizer;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Attachments.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// The attachment controller.
    /// </summary>
    public partial class AttachmentController : AppController
    {
        /// <summary>
        /// The sizes dictionary.
        /// </summary>
        private readonly IDictionary<string, string> sizesDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public AttachmentController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
            this.sizesDictionary = new Dictionary<string, string>
            {
                { "avatar", "width=64&height=64&crop=auto" },
                { "xs", "width=100&height=75&crop=auto" },
                { "s", "width=320&height=240&crop=auto" },
                { "m", "width=640&height=480&crop=auto" },
                { "l", "maxwidth=800&maxheight=600" },
                { "xl", "maxwidth=1024&maxheight=768" },
                { "xxl", "maxwidth=1680&maxheight=1050" }
            };
        }

        /// <inheritdoc />
        public virtual ActionResult Index(int attachmentId, string size)
        {
            var attachment = this.QueryProcessor.Process(new GetAttachmentInfoQuery { AttachmentId = attachmentId });

            this.Response.Cache.SetCacheability(HttpCacheability.Public);
            this.Response.Cache.SetMaxAge(TimeSpan.FromDays(7));
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(7));

            if (!attachment.IsImage || string.IsNullOrEmpty(size) || !this.sizesDictionary.ContainsKey(size))
            {
                return this.File(attachment.FilePath, attachment.ContentType, attachment.IsImage ? null : attachment.FileName);
            }

            string targetPath = string.Format("{0}-{1}.thb", attachment.FilePath, size);

            if (!System.IO.File.Exists(targetPath))
            {
                var resizeCropSettings = new ResizeSettings(this.sizesDictionary[size]);

                if (attachment.ContentType.Contains("png"))
                {
                    resizeCropSettings["colors"] = "256";
                }

                ImageBuilder.Current.Build(attachment.FilePath, targetPath, resizeCropSettings);
            }

            return this.File(targetPath, attachment.ContentType, null);
        }

        /// <inheritdoc />
        [HttpPost]
        [Authorize(Roles = "User")]
        [AntiForgery]
        public virtual JsonResult Upload()
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