// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AppSettings.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Settings
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;

    /// <summary>
    /// Provides an access to the settings from appConfig section.
    /// </summary>
    public sealed class AppSettings : ISettings
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly IDictionary<string, string> settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings()
        {
            this.settings = new Dictionary<string, string>();

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                this.settings[key] = ConfigurationManager.AppSettings[key];
            }
        }

        /// <inheritdoc />
        public T GetValue<T>(string configKey)
        {
            if (!this.settings.ContainsKey(configKey))
            {
                throw new SettingsPropertyNotFoundException(
                    string.Format("Configuration value with key \"{0}\" have not been found", configKey));
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFrom(this.settings[configKey]);
        }

        /// <inheritdoc />
        public T GetValue<T>(string configKey, T defaultValue)
        {
            return this.settings.ContainsKey(configKey) ? this.GetValue<T>(configKey) : defaultValue;
        }
    }
}
