// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommandsConverter.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The queries converter.
    /// </summary>
    public class CommandsConverter : JsonConverter
    {
        /// <summary>
        /// The query types.
        /// </summary>
        private static readonly IDictionary<string, Func<Command>> CommandFactory;

        /// <summary>
        /// Initializes static members of the <see cref="CommandsConverter"/> class.
        /// </summary>
        static CommandsConverter()
        {
            CommandFactory = new Dictionary<string, Func<Command>>();

            var queryTypes =
                AppDomain.CurrentDomain.GetAssemblies().Where(
                    a => a.FullName.StartsWith("MirGames.Domain", StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(
                        a =>
                        a.GetTypes().Where(
                            myType =>
                            myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Command))
                            && myType.GetCustomAttributes(typeof(ApiAttribute), true).Any()))
                    .ToArray();

            foreach (var queryType in queryTypes)
            {
                NewExpression newExp = Expression.New(queryType);
                LambdaExpression lambda = Expression.Lambda(typeof(Func<Command>), newExp);

                var compiled = (Func<Command>)lambda.Compile();
                CommandFactory.Add(queryType.Name, compiled);
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
                throw new JsonReaderException("Type of the command isn't specified.");
            }

            var commandType = typeToken.Value<string>();

            if (!CommandFactory.ContainsKey(commandType))
            {
                throw new JsonReaderException("Wrong type of the command.");
            }

            var target = CommandFactory[commandType].Invoke();
            serializer.Populate(jsonObject.CreateReader(), target);

            return target;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Command);
        }
    }
}