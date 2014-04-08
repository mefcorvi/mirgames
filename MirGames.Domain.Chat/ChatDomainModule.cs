namespace MirGames.Domain.Chat
{
    using Autofac;

    using MirGames.Domain.Chat.Services;
    using MirGames.Domain.Chat.UserSettings;
    using MirGames.Domain.Users.Services;

    /// <summary>
    /// The domain module.
    /// </summary>
    public sealed class ChatDomainModule : DomainModuleBase
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UseEnterToSendChatMessageSetting>().As<IUserSettingHandler>().SingleInstance();
            builder.RegisterType<UseWebSocketSetting>().As<IUserSettingHandler>().SingleInstance();

            builder.RegisterType<ChatMessageUploadProcessor>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EntityMapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
