// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ContentTypeProvider.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
        /// Initializes a new instance of the <see cref="ContentTypeProvider" /> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public ContentTypeProvider(ICacheManagerFactory cacheManagerFactory)
        {
            this.cacheManager = cacheManagerFactory.Create("Common");
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