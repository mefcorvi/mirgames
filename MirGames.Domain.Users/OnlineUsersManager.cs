namespace MirGames.Domain.Users
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Events;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Manages the online users.
    /// </summary>
    internal sealed class OnlineUsersManager : IOnlineUsersManager, IDisposable
    {
        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The avatar provider.
        /// </summary>
        private readonly IAvatarProvider avatarProvider;

        /// <summary>
        /// The online users.
        /// </summary>
        private readonly ConcurrentDictionary<int, OnlineUserContainer> onlineUsers;

        /// <summary>
        /// The timer.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineUsersManager" /> class.
        /// </summary>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="avatarProvider">The avatar provider.</param>
        public OnlineUsersManager(IEventBus eventBus, IReadContextFactory readContextFactory, IAvatarProvider avatarProvider)
        {
            Contract.Requires(eventBus != null);
            Contract.Requires(readContextFactory != null);
            Contract.Requires(avatarProvider != null);

            this.eventBus = eventBus;
            this.readContextFactory = readContextFactory;
            this.avatarProvider = avatarProvider;
            this.onlineUsers = new ConcurrentDictionary<int, OnlineUserContainer>();
            this.timer = new Timer(this.CheckAliveUsers, null, 0, 5000);
        }

        /// <inheritdoc />
        public void Ping(ClaimsPrincipal principal)
        {
            int? userId = principal.GetUserId();

            if (userId.HasValue)
            {
                this.onlineUsers.AddOrUpdate(
                    userId.Value,
                    id =>
                        {
                            var user = this.LoadUser(id);
                            this.eventBus.Raise(new UserOnlineEvent { UserId = id });

                            return new OnlineUserContainer(user);
                        },
                    (idx, oldItem) =>
                        {
                            oldItem.ViewModel.LastRequestDate = DateTime.UtcNow;
                            return oldItem;
                        });
            }
        }

        /// <inheritdoc />
        public void MarkUserAsOffline(ClaimsPrincipal principal)
        {
            int? userId = principal.GetUserId();

            if (userId.HasValue)
            {
                this.MarkUserAsOffline(userId.Value);
            }
        }

        /// <inheritdoc />
        public void AddUserTag(ClaimsPrincipal principal, string tag, TimeSpan? expirationTime)
        {
            this.Ping(principal);
            int? userId = principal.GetUserId();

            if (userId.HasValue)
            {
                OnlineUserContainer onlineUser;
                if (this.onlineUsers.TryGetValue(userId.Value, out onlineUser))
                {
                    onlineUser.AddTag(tag, expirationTime);
                }
            }
        }

        /// <inheritdoc />
        public void RemoveUserTag(ClaimsPrincipal principal, string tag)
        {
            this.Ping(principal);
            int? userId = principal.GetUserId();

            if (userId.HasValue)
            {
                OnlineUserContainer onlineUser;
                if (this.onlineUsers.TryGetValue(userId.Value, out onlineUser))
                {
                    onlineUser.RemoveTag(tag);
                }
            }
        }

        /// <inheritdoc />
        public IDictionary<int, IEnumerable<string>> GetUserTags()
        {
            return this.onlineUsers.Values
                .Where(x => x.ViewModel.Id.HasValue)
                .ToDictionary(x => x.ViewModel.Id.GetValueOrDefault(), x => x.Tags);
        }

        /// <inheritdoc />
        public IEnumerable<OnlineUserViewModel> GetOnlineUsers()
        {
            return this.onlineUsers.Values.Select(x => x.ViewModel).ToArray();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.timer.Dispose();
        }

        /// <summary>
        /// Checks the alive users.
        /// </summary>
        /// <param name="state">The state.</param>
        private void CheckAliveUsers(object state)
        {
            var keys = this.onlineUsers.Keys;
            DateTime onlineBoundary = DateTime.UtcNow.AddMinutes(-5);

            foreach (var key in keys)
            {
                OnlineUserContainer onlineUserViewModel;
                if (this.onlineUsers.TryGetValue(key, out onlineUserViewModel))
                {
                    if (onlineUserViewModel.ViewModel.LastRequestDate < onlineBoundary)
                    {
                        this.MarkUserAsOffline(key);
                    }

                    this.RemoveExpiredTags(onlineUserViewModel);
                }
            }
        }

        /// <summary>
        /// Removes the expired tags.
        /// </summary>
        /// <param name="onlineUserViewModel">The online user view model.</param>
        private void RemoveExpiredTags(OnlineUserContainer onlineUserViewModel)
        {
            var removedTags = onlineUserViewModel.RemoveExpiredTags();

            foreach (string tag in removedTags)
            {
                this.eventBus.Raise(
                    new OnlineUserTagRemovedEvent
                        {
                            Tag = tag,
                            UserId = onlineUserViewModel.ViewModel.Id.GetValueOrDefault()
                        });
            }
        }

        /// <summary>
        /// Marks the user as offline.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        private void MarkUserAsOffline(int userId)
        {
            OnlineUserContainer onlineUserViewModel;
            if (this.onlineUsers.TryRemove(userId, out onlineUserViewModel))
            {
                this.eventBus.Raise(
                    new UserOfflineEvent
                        {
                            UserId = userId
                        });
            }
        }

        /// <summary>
        /// Loads the user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>The user.</returns>
        private OnlineUserViewModel LoadUser(int userId)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var user = readContext.Query<User>().Where(u => u.Id == userId).Select(
                    u => new
                        {
                            u.AvatarUrl,
                            u.Id,
                            u.Login,
                            u.Mail
                        }).FirstOrDefault();

                if (user == null)
                {
                    throw new ItemNotFoundException("User", userId);
                }

                return new OnlineUserViewModel
                    {
                        AvatarUrl = user.AvatarUrl ?? this.avatarProvider.GetAvatarUrl(user.Mail, user.Login),
                        Id = user.Id,
                        LastRequestDate = DateTime.UtcNow,
                        SessionDate = DateTime.UtcNow,
                        Login = user.Login
                    };
            }
        }

        /// <summary>
        /// The online user container.
        /// </summary>
        private class OnlineUserContainer
        {
            /// <summary>
            /// The lock object.
            /// </summary>
            private readonly object lockObject = new object();

            /// <summary>
            /// The tags.
            /// </summary>
            private readonly IDictionary<string, DateTimeOffset?> tags;

            /// <summary>
            /// Initializes a new instance of the <see cref="OnlineUserContainer"/> class.
            /// </summary>
            /// <param name="viewModel">The view model.</param>
            public OnlineUserContainer(OnlineUserViewModel viewModel)
            {
                this.ViewModel = viewModel;
                this.tags = new Dictionary<string, DateTimeOffset?>();
            }

            /// <summary>
            /// Gets the view model.
            /// </summary>
            public OnlineUserViewModel ViewModel { get; private set; }

            /// <summary>
            /// Gets the tags.
            /// </summary>
            public IEnumerable<string> Tags
            {
                get { return this.tags.Keys; }
            }

            /// <summary>
            /// Adds the tag.
            /// </summary>
            /// <param name="tag">The tag.</param>
            /// <param name="expirationTime">The expiration time.</param>
            public void AddTag(string tag, TimeSpan? expirationTime)
            {
                lock (this.lockObject)
                {
                    this.tags[tag] = expirationTime.HasValue ? DateTimeOffset.Now.Add(expirationTime.Value) : (DateTimeOffset?)null;
                }
            }

            /// <summary>
            /// Removes the tag.
            /// </summary>
            /// <param name="tag">The tag.</param>
            public void RemoveTag(string tag)
            {
                lock (this.lockObject)
                {
                    this.tags.Remove(tag);
                }
            }

            /// <summary>
            /// Removes the expired tags.
            /// </summary>
            /// <returns>Sequence of removed tags.</returns>
            public IEnumerable<string> RemoveExpiredTags()
            {
                lock (this.lockObject)
                {
                    var tagsToRemove = this.tags
                        .Where(pair => pair.Value.HasValue && pair.Value.Value < DateTimeOffset.Now)
                        .Select(pair => pair.Key)
                        .ToArray();

                    tagsToRemove.ForEach(tag => this.tags.Remove(tag));

                    return tagsToRemove;
                }
            }
        }
    }
}