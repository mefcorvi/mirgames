namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The friend map.
    /// </summary>
    public class FriendMap : EntityTypeConfiguration<friend>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendMap"/> class.
        /// </summary>
        public FriendMap()
        {
            // Primary Key
            this.HasKey(t => new { t.user_from, t.user_to });

            // Properties
            this.Property(t => t.user_from).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_to).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_friend", "social");
            this.Property(t => t.user_from).HasColumnName("user_from");
            this.Property(t => t.user_to).HasColumnName("user_to");
            this.Property(t => t.status_from).HasColumnName("status_from");
            this.Property(t => t.status_to).HasColumnName("status_to");

            // Relationships
            this.HasRequired(t => t.user).WithMany(t => t.Friend).HasForeignKey(d => d.user_from);
            this.HasRequired(t => t.user1).WithMany(t => t.friend1).HasForeignKey(d => d.user_to);
        }

        #endregion
    }
}