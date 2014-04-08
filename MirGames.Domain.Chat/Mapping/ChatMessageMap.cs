namespace MirGames.Domain.Chat.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Chat.Entities;

    /// <summary>
    /// The forum post mapping.
    /// </summary>
    internal sealed class ChatMessageMap : EntityTypeConfiguration<ChatMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageMap"/> class.
        /// </summary>
        public ChatMessageMap()
        {
            this.ToTable("chat_messages");

            this.HasKey(t => t.MessageId);

            this.Property(t => t.MessageId).HasColumnName("message_id");
            this.Property(t => t.AuthorId).HasColumnName("author_id");
            this.Property(t => t.AuthorLogin).HasColumnName("author_login");
            this.Property(t => t.AuthorIp).IsRequired().HasMaxLength(48).HasColumnName("author_ip");
            this.Property(t => t.CreatedDate).HasColumnName("created_date");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.Message).IsRequired().HasColumnName("message");
        }
    }
}