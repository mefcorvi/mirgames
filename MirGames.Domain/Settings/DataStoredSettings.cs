// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DataStoredSettings.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using MirGames.Domain.Entities;
    using MirGames.Infrastructure;

    /// <summary>
    /// Provides an access to the configuration stored in the data context.
    /// </summary>
    internal sealed class DataStoredSettings : ISettings
    {
        /// <summary>
        /// The inner settings.
        /// </summary>
        private readonly ISettings innerSettings;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStoredSettings" /> class.
        /// </summary>
        /// <param name="innerSettings">The inner settings.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public DataStoredSettings(ISettings innerSettings, IReadContextFactory readContextFactory)
        {
            this.innerSettings = innerSettings;
            this.readContextFactory = readContextFactory;
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
            ConfigItem configItem;

            using (var readContext = this.readContextFactory.Create())
            {
                configItem = readContext.Query<ConfigItem>().FirstOrDefault(item => item.Key == configKey);
            }

            if (configItem == null)
            {
                return innerCall(this.innerSettings);
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFrom(configItem.Value);
        }
    }
}
