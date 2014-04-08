namespace MirGames.Domain.Users.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// The user_administrator map.
    /// </summary>
    internal class UserAdministratorMap : EntityTypeConfiguration<UserAdministrator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAdministratorMap"/> class.
        /// </summary>
        public UserAdministratorMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_user_administrator");
            this.Property(t => t.UserId).HasColumnName("user_id");
        }
    }
}