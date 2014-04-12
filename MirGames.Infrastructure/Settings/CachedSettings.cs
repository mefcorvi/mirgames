// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CachedSettings.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Settings
{
    using System;
    using System.Diagnostics.Contracts;

    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Cache;

    /// <summary>
    /// Provides an access to the configuration cached in-memory.
    /// </summary>
    public sealed class CachedSettings : ISettings
    {
        /// <summary>
        /// The inner settings.
        /// </summary>
        private readonly ISettings innerSettings;

        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSettings" /> class.
        /// </summary>
        /// <param name="innerSettings">The inner settings.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public CachedSettings(ISettings innerSettings, ICacheManager cacheManager)
        {
            Contract.Requires(innerSettings != null);
            Contract.Requires(cacheManager != null);

            this.innerSettings = innerSettings;
            this.cacheManager = cacheManager;
        }

        /// <inheritdoc />
        public T GetValue<T>(string configKey)
        {
            return this.DecorateInnerCall(configKey, inner => inner.GetValue<T>(configKey));
        }

        /// <inheritdoc />
        public T GetValue<T>(string configKey, T defaultValue)
        {
            return this.DecorateInnerCall(configKey, inner => inner.GetValue(configKey, defaultValue));
        }

        /// <summary>
        /// Decorates the inner call.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="configKey">The configuration key.</param>
        /// <param name="innerCall">The inner call.</param>
        /// <returns>The value.</returns>
        private T DecorateInnerCall<T>(string configKey, Func<ISettings, T> innerCall)
        {
            string cacheKey = string.Format("Settings." + configKey);

            return this.cacheManager.GetOrAdd(
                cacheKey, () => innerCall(this.innerSettings), DateTimeOffset.Now.AddMinutes(10));
        }
    }
}