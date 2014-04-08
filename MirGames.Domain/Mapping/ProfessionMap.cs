namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The profession map.
    /// </summary>
    public class ProfessionMap : EntityTypeConfiguration<Profession>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfessionMap"/> class.
        /// </summary>
        public ProfessionMap()
        {
            // Primary Key
            this.HasKey(t => t.prof_id);

            // Properties
            this.Property(t => t.prof_name).IsRequired().HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("prefix_prof", "social");
            this.Property(t => t.prof_id).HasColumnName("prof_id");
            this.Property(t => t.prof_name).HasColumnName("prof_name");
        }

        #endregion
    }
}