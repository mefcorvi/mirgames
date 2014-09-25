namespace MirGames.Domain.Wip.Services
{
    using System;
    using System.IO;

    using ImageResizer;

    using MirGames.Domain.Attachments.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The upload processor.
    /// </summary>
    internal sealed class ProjectDescriptionUploadProcessor : IUploadProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            return entityType.EqualsIgnoreCase("project-description");
        }

        /// <inheritdoc />
        public bool IsValid(string filePath)
        {
            try
            {
                ImageBuilder.Current.LoadImageInfo(filePath, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public void Process(string filePath)
        {
            string tempFile = Path.GetTempFileName();

            var resizeCropSettings = new ResizeSettings("width=800&height=600&mode=max");
            ImageBuilder.Current.Build(filePath, tempFile, resizeCropSettings);
            File.Copy(tempFile, filePath, true);
            File.Delete(tempFile);
        }
    }
}