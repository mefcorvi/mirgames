// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetMentionsFromTextQueryHandler.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Finds mentions and transforms them.
    /// </summary>
    internal sealed class GetMentionsFromTextQueryHandler : SingleItemQueryHandler<GetMentionsFromTextQuery, MentionsInTextViewModel>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMentionsFromTextQueryHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetMentionsFromTextQueryHandler(IReadContextFactory readContextFactory, IQueryProcessor queryProcessor)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(queryProcessor != null);

            this.readContextFactory = readContextFactory;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override MentionsInTextViewModel Execute(GetMentionsFromTextQuery command, ClaimsPrincipal principal)
        {
            if (string.IsNullOrWhiteSpace(command.Text))
            {
                return new MentionsInTextViewModel
                {
                    TransformedText = command.Text
                };
            }

            var resolvedUsers = new List<AuthorViewModel>();
            var resultBuilder = new StringBuilder(command.Text.Length);

            for (int i = 0; i < command.Text.Length; i++)
            {
                char c = command.Text[i];

                if (c == '@')
                {
                    int j = i + 1;

                    while (j < command.Text.Length && !(char.IsWhiteSpace(command.Text[j]) || char.IsPunctuation(command.Text[j])))
                    {
                        j++;
                    }

                    if (j - i > 1)
                    {
                        string s = command.Text.Substring(i + 1, j - i - 1);
                        var users = this.GetAssumptedUsers(s);

                        int maxLength = 0;
                        AuthorViewModel assumptedUser = null;

                        foreach (var user in users)
                        {
                            if (user.Login.Length > maxLength && (command.Text.Length - i) >= user.Login.Length)
                            {
                                string substring = command.Text.Substring(i + 1, user.Login.Length);
                                int nextIndex = i + 1 + user.Login.Length;
                                char next = nextIndex < command.Text.Length
                                                ? command.Text[nextIndex]
                                                : ' ';

                                if (substring == user.Login && (char.IsWhiteSpace(next) || char.IsPunctuation(next)))
                                {
                                    maxLength = user.Login.Length;
                                    assumptedUser = user;
                                }
                            }
                        }

                        if (assumptedUser != null)
                        {
                            resultBuilder.AppendFormat(
                                "<user id=\"{0}\">{1}</user>",
                                assumptedUser.Id,
                                assumptedUser.Login);

                            resolvedUsers.Add(assumptedUser);

                            i += assumptedUser.Login.Length;
                            continue;
                        }
                    }
                }

                resultBuilder.Append(c);
            }

            resolvedUsers = this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = resolvedUsers
            }).ToList();

            return new MentionsInTextViewModel
            {
                TransformedText = resultBuilder.ToString(),
                Users = resolvedUsers
            };
        }

        /// <summary>
        /// Gets the assumpted users.
        /// </summary>
        /// <param name="partOfLogin">The part of login.</param>
        /// <returns>The assumpted users.</returns>
        private IEnumerable<AuthorViewModel> GetAssumptedUsers(string partOfLogin)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return
                    readContext
                        .Query<User>()
                        .Where(u => u.Login.StartsWith(partOfLogin))
                        .Select(u => new AuthorViewModel
                        {
                            Id = u.Id,
                            Login = u.Login
                        })
                        .ToList();
            }
        }
    }
}
