namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Finds mentions and transforms them.
    /// </summary>
    internal sealed class TransformMentionsCommandHandler : CommandHandler<TransformMentionsCommand, string>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMentionsCommandHandler"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public TransformMentionsCommandHandler(IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        public override string Execute(TransformMentionsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            if (string.IsNullOrWhiteSpace(command.Text))
            {
                return command.Text;
            }

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
                        UserViewModel assumptedUser = null;

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

                            i += assumptedUser.Login.Length;
                            continue;
                        }
                    }
                }

                resultBuilder.Append(c);
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Gets the assumpted users.
        /// </summary>
        /// <param name="partOfLogin">The part of login.</param>
        /// <returns>The assumpted users.</returns>
        private IEnumerable<UserViewModel> GetAssumptedUsers(string partOfLogin)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return
                    readContext
                        .Query<User>()
                        .Where(u => u.Login.StartsWith(partOfLogin))
                        .Select(u => new UserViewModel
                        {
                            Id = u.Id,
                            Login = u.Login
                        })
                        .ToList();
            }
        }
    }
}
