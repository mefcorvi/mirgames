// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueriesConverter.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using MirGames.Infrastructure.Commands;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The queries converter.
    /// </summary>
    public class QueriesConverter : JsonConverter
    {
        /// <summary>
        /// The query types.
        /// </summary>
        private static readonly IDictionary<string, Func<Query>> QueryFactory;

        /// <summary>
        /// Initializes static members of the <see cref="QueriesConverter"/> class.
        /// </summary>
        static QueriesConverter()
        {
            QueryFactory = new Dictionary<string, Func<Query>>();

            var queryTypes =
                AppDomain.CurrentDomain.GetAssemblies().Where(
                    a => a.FullName.StartsWith("MirGames.Domain", StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(
                        a =>
                        a.GetTypes().Where(
                            myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Query))
                                      && myType.GetCustomAttributes(typeof(ApiAttribute), true).Any()))
                    .ToArray();

            foreach (var queryType in queryTypes)
            {
                NewExpression newExp = Expression.New(queryType);
                LambdaExpression lambda = Expression.Lambda(typeof(Func<Query>), newExp);

                var compiled = (Func<Query>)lambda.Compile();
                QueryFactory.Add(queryType.Name, compiled);
            }
        }

        /// <inheritdoc />
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jsonObject = JObject.Load(reader);

            var typeToken = jsonObject.GetValue("$_type");
            if (typeToken.Type != JTokenType.String)
            {
                throw new JsonReaderException("Type of the query isn't specified.");
            }

            var queryType = typeToken.Value<string>();

            if (!QueryFactory.ContainsKey(queryType))
            {
                throw new JsonReaderException("Wrong type of the query.");
            }

            var target = QueryFactory[queryType].Invoke();
            serializer.Populate(jsonObject.CreateReader(), target);

            return target;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Query);
        }
    }
}