namespace MirGames.Infrastructure.Utilities
{
    using System;

    using Microsoft.Win32;

    using MirGames.Infrastructure.Cache;

    /// <summary>
    /// Provider content-type by the file extension.
    /// </summary>
    internal sealed class ContentTypeProvider : IContentTypeProvider
    {
        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeProvider"/> class.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        public ContentTypeProvider(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <inheritdoc />
        public string GetContentType(string fileExtension)
        {
            return this.cacheManager.GetOrAdd(
                "ContentType" + fileExtension,
                () => GetContentTypeFromRegistry(fileExtension),
                DateTimeOffset.Now.AddDays(1));
        }

        /// <summary>
        /// Gets the content type from registry.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>The content type.</returns>
        private static string GetContentTypeFromRegistry(string fileExtension)
        {
            const string DefaultContentType = "application/octet-stream";

            try
            {
                using (var regkey = Registry.ClassesRoot)
                {
                    using (var fileextkey = regkey.OpenSubKey(fileExtension))
                    {
                        return fileextkey != null
                                   ? fileextkey.GetValue("Content Type", DefaultContentType).ToString()
                                   : DefaultContentType;
                    }
                }
            }
            catch
            {
                return DefaultContentType;
            }
        }
    }
}