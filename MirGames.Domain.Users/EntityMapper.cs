namespace MirGames.Domain.Users
{
    using MirGames.Domain.Users.Mapping;
    using MirGames.Infrastructure;

    /// <summary>
    /// The entity mapper.
    /// </summary>
    internal sealed class EntityMapper : IEntityMapper
    {
        /// <inheritdoc />
        public void Configure(IEntityMappingRegistry registry)
        {
            registry.Register(new UserSessionMap());
            registry.Register(new UserMap());
            registry.Register(new PasswordRestoreRequestMap());

            registry.Register(new UserAdministratorMap());
            registry.Register(new WallRecordMap());

            registry.Register(new OAuthTokenMap());
            registry.Register(new OAuthTokenDataMap());
            registry.Register(new OAuthProviderMap());
        }
    }
}