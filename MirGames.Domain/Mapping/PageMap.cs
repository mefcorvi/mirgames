namespace MirGames.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Entities;

    /// <summary>
    /// The page map.
    /// </summary>
    public class PageMap : EntityTypeConfiguration<page>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PageMap"/> class.
        /// </summary>
        public PageMap()
        {
            // Primary Key
            this.HasKey(t => t.page_id);

            // Properties
            this.Property(t => t.page_url).IsRequired().HasMaxLength(50);

            this.Property(t => t.page_url_full).IsRequired().HasMaxLength(254);

            this.Property(t => t.page_title).IsRequired().HasMaxLength(200);

            this.Property(t => t.page_text).IsRequired().HasMaxLength(65535);

            this.Property(t => t.page_seo_keywords).HasMaxLength(250);

            this.Property(t => t.page_seo_description).HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("prefix_page", "social");
            this.Property(t => t.page_id).HasColumnName("page_id");
            this.Property(t => t.page_pid).HasColumnName("page_pid");
            this.Property(t => t.page_url).HasColumnName("page_url");
            this.Property(t => t.page_url_full).HasColumnName("page_url_full");
            this.Property(t => t.page_title).HasColumnName("page_title");
            this.Property(t => t.page_text).HasColumnName("page_text");
            this.Property(t => t.page_date_add).HasColumnName("page_date_add");
            this.Property(t => t.page_date_edit).HasColumnName("page_date_edit");
            this.Property(t => t.page_seo_keywords).HasColumnName("page_seo_keywords");
            this.Property(t => t.page_seo_description).HasColumnName("page_seo_description");
            this.Property(t => t.page_active).HasColumnName("page_active");
            this.Property(t => t.page_main).HasColumnName("page_main");
            this.Property(t => t.page_sort).HasColumnName("page_sort");
            this.Property(t => t.page_auto_br).HasColumnName("page_auto_br");
        }

        #endregion
    }
}