namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// Mapping of the invite entity.
    /// </summary>
    internal sealed class InviteMap : EntityTypeConfiguration<invite>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InviteMap"/> class.
        /// </summary>
        public InviteMap()
        {
            // Primary Key
            this.HasKey(t => t.invite_id);

            // Properties
            this.Property(t => t.invite_code)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("prefix_invite", "social");
            this.Property(t => t.invite_id).HasColumnName("invite_id");
            this.Property(t => t.invite_code).HasColumnName("invite_code");
            this.Property(t => t.user_from_id).HasColumnName("user_from_id");
            this.Property(t => t.user_to_id).HasColumnName("user_to_id");
            this.Property(t => t.invite_date_add).HasColumnName("invite_date_add");
            this.Property(t => t.invite_date_used).HasColumnName("invite_date_used");
            this.Property(t => t.invite_used).HasColumnName("invite_used");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.invite)
                .HasForeignKey(d => d.user_from_id);
            this.HasOptional(t => t.user1)
                .WithMany(t => t.invite1)
                .HasForeignKey(d => d.user_to_id);
        }
    }
}
