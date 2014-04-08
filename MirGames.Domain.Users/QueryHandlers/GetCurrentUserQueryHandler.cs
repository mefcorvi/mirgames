namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.Services;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    using Newtonsoft.Json;

    /// <summary>
    /// Handles the GetCurrentUserQuery.
    /// </summary>
    internal sealed class GetCurrentUserQueryHandler : SingleItemQueryHandler<GetCurrentUserQuery, CurrentUserViewModel>
    {
        /// <summary>
        /// The user setting handlers.
        /// </summary>
        private readonly IEnumerable<IUserSettingHandler> userSettingHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentUserQueryHandler"/> class.
        /// </summary>
        /// <param name="userSettingHandlers">The user setting handlers.</param>
        public GetCurrentUserQueryHandler(IEnumerable<IUserSettingHandler> userSettingHandlers)
        {
            this.userSettingHandlers = userSettingHandlers;
        }

        /// <inheritdoc />
        public override CurrentUserViewModel Execute(IReadContext readContext, GetCurrentUserQuery query, ClaimsPrincipal principal)
        {
            var userId = principal.GetUserId().GetValueOrDefault();
            var user = readContext.Query<User>().Where(u => u.Id == userId).Select(
                u => new
                    {
                        u.AvatarUrl,
                        u.Id,
                        u.Login,
                        u.Name,
                        u.SettingsJson,
                        u.Timezone,
                        u.IsActivated
                    }).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(user.SettingsJson);
            var settingsViewModel = new Dictionary<string, object>();

            foreach (IUserSettingHandler settingHandler in userSettingHandlers)
            {
                string key = settingHandler.SettingKey;
                settingsViewModel[key] = settingHandler.ToViewModel(settings.ContainsKey(key) ? settings[key] : null);
            }

            settingsViewModel["TimeZone"] = user.Timezone;

            return new CurrentUserViewModel
                {
                    AvatarUrl = user.AvatarUrl,
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name,
                    Settings = settingsViewModel,
                    IsActivated = user.IsActivated,
                    TimeZone = user.Timezone
                };
        }
    }
}