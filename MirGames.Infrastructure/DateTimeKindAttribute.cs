// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DateTimeKindAttribute.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Specifies the kind of the DateTime property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeKindAttribute : Attribute
    {
        /// <summary>
        /// The kind.
        /// </summary>
        private readonly DateTimeKind kind;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeKindAttribute"/> class.
        /// </summary>
        /// <param name="kind">The kind.</param>
        public DateTimeKindAttribute(DateTimeKind kind)
        {
            this.kind = kind;
        }

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public DateTimeKind Kind
        {
            get { return this.kind; }
        }

        /// <summary>
        /// Processes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void Apply(object entity)
        {
            if (entity == null)
            {
                return;
            }

            // TODO: optimize
            var properties = entity.GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?));

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<DateTimeKindAttribute>();
                if (attr == null)
                {
                    continue;
                }

                var dt = property.PropertyType == typeof(DateTime?)
                             ? (DateTime?)property.GetValue(entity)
                             : (DateTime)property.GetValue(entity);

                if (dt == null)
                {
                    continue;
                }

                property.SetValue(entity, DateTime.SpecifyKind(dt.Value, attr.Kind));
            }
        }
    }
}