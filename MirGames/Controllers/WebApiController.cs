// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WebApiController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using System.Web.Http;

    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    using Newtonsoft.Json;

    /// <summary>
    /// The API controller.
    /// </summary>
    public class WebApiController : ApiController
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The authentication provider.
        /// </summary>
        private readonly IAuthenticationProvider authenticationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="authenticationProvider">The authentication provider.</param>
        public WebApiController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor, IAuthenticationProvider authenticationProvider)
        {
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
            this.authenticationProvider = authenticationProvider;
        }

        /// <inheritdoc />
        public IEnumerable<object> GetAll(string query, int pageNum, int pageSize, string sessionId = null)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(query), "Query should be specified in JSON format.");
            Contract.Requires(pageSize <= 100, "Maximum available page size is 100.");
            Contract.Requires(pageNum >= 0, "Page number could not be a negative integer");

            this.Authenticate(sessionId);

            var queryInstance = JsonConvert.DeserializeObject<Query>(query);

            return this.queryProcessor.Process(queryInstance, new PaginationSettings(pageNum, pageSize));
        }

        /// <inheritdoc />
        public object GetOne(string query, string sessionId = null)
        {
            this.Authenticate(sessionId);

            var queryInstance = JsonConvert.DeserializeObject<Query>(query);
            return this.queryProcessor.Process(queryInstance, new PaginationSettings(0, 1)).FirstOrDefault();
        }

        /// <inheritdoc />
        public object Post([FromBody]ApiPostModel model)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(model.Command), "Command should be specified in JSON format.");

            this.Authenticate(model.SessionId);

            var commandInstance = JsonConvert.DeserializeObject<Command>(model.Command);
            var result = this.commandProcessor.ExecuteWithResult(commandInstance);

            return result;
        }

        /// <summary>
        /// The API post model.
        /// </summary>
        public class ApiPostModel
        {
            /// <summary>
            /// Gets or sets the command.
            /// </summary>
            public string Command { get; set; }

            /// <summary>
            /// Gets or sets the session unique identifier.
            /// </summary>
            public string SessionId { get; set; }
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        private void Authenticate(string sessionId)
        {
            var principal = this.authenticationProvider.GetPrincipal(sessionId);
            this.User = principal;
            Thread.CurrentPrincipal = principal;
        }
    }
}
