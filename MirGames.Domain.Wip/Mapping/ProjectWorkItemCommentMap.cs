namespace MirGames.Domain.Wip.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Wip.Entities;

    /// <summary>
    /// The project work item comment map.
    /// </summary>
    internal sealed class ProjectWorkItemCommentMap : EntityTypeConfiguration<ProjectWorkItemComment>
    {
        public ProjectWorkItemCommentMap()
        {
            this.HasKey(t => t.CommentId);
            this.ToTable("wip_work_item_comments");

            this.Property(t => t.CommentId).HasColumnName("comment_id");
            this.Property(t => t.Date).HasColumnName("date");
            this.Property(t => t.Text).HasColumnName("text");
            this.Property(t => t.UpdatedDate).HasColumnName("updated_date");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.UserIp).HasColumnName("user_ip");
            this.Property(t => t.UserLogin).HasColumnName("user_login");
            this.Property(t => t.WorkItemId).HasColumnName("work_item_id");
        }
    }
}