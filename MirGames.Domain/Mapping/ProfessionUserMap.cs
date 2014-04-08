namespace MirGames.Domain.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The profession user map.
    /// </summary>
    public class ProfessionUserMap : EntityTypeConfiguration<prof_user>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfessionUserMap"/> class.
        /// </summary>
        public ProfessionUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.prof_id, t.user_id });

            // Properties
            this.Property(t => t.prof_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("prefix_prof_user", "social");
            this.Property(t => t.prof_id).HasColumnName("prof_id");
            this.Property(t => t.user_id).HasColumnName("user_id");
        }

        #endregion
    }
}