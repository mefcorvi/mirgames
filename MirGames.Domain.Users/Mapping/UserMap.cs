namespace MirGames.Domain.Users.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using MirGames.Domain.Users.Entities;

    /// <summary>
    /// Mapping of the user.
    /// </summary>
    internal sealed class UserMap : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMap"/> class.
        /// </summary>
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Ignore(t => t.Settings);
            this.Ignore(t => t.TimezoneInfo);

            // Properties
            this.Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SettingsJson)
                .HasColumnName("settings")
                .HasMaxLength(1024);

            this.Property(t => t.Mail)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RegistrationIP)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserActivationKey)
                .HasMaxLength(32);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Gender)
                .IsRequired()
                .HasMaxLength(65532);

            this.Property(t => t.Location)
                .HasMaxLength(255);

            this.Property(t => t.About)
                .HasMaxLength(65535);

            this.Property(t => t.AvatarUrl)
                .HasMaxLength(250);

            this.Property(t => t.PhotoUrl)
                .HasMaxLength(250);

            this.Property(t => t.Timezone)
                .HasMaxLength(64);

            this.Property(t => t.LastVisitDate)
                .HasColumnName("last_visit")
                .IsRequired();

            this.Property(t => t.PasswordSalt)
                .HasColumnName("salt")
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("prefix_user");
            this.Property(t => t.Id).HasColumnName("user_id");
            this.Property(t => t.Login).HasColumnName("user_login");
            this.Property(t => t.Password).HasColumnName("user_password");
            this.Property(t => t.Mail).HasColumnName("user_mail");
            this.Property(t => t.RegistrationDate).HasColumnName("user_date_register");
            this.Property(t => t.ActivationDate).HasColumnName("user_date_activate");
            this.Property(t => t.LastCommentDate).HasColumnName("user_date_comment_last");
            this.Property(t => t.RegistrationIP).HasColumnName("user_ip_register");
            this.Property(t => t.UserRating).HasColumnName("user_rating");
            this.Property(t => t.UserCountVote).HasColumnName("user_count_vote");
            this.Property(t => t.IsActivated).HasColumnName("user_activate");
            this.Property(t => t.UserActivationKey).HasColumnName("user_activate_key");
            this.Property(t => t.Name).HasColumnName("user_profile_name");
            this.Property(t => t.Gender).HasColumnName("user_profile_sex");
            this.Property(t => t.Location).HasColumnName("user_profile_location");
            this.Property(t => t.Birthday).HasColumnName("user_profile_birthday");
            this.Property(t => t.About).HasColumnName("user_profile_about");
            this.Property(t => t.UpdateDate).HasColumnName("user_profile_date");
            this.Property(t => t.AvatarUrl).HasColumnName("user_profile_avatar");
            this.Property(t => t.PhotoUrl).HasColumnName("user_profile_foto");
            this.Property(t => t.NoticeNewTopic).HasColumnName("user_settings_notice_new_topic");
            this.Property(t => t.NoticeNewComment).HasColumnName("user_settings_notice_new_comment");
            this.Property(t => t.NoticeNewTalk).HasColumnName("user_settings_notice_new_talk");
            this.Property(t => t.NoticeReplyComment).HasColumnName("user_settings_notice_reply_comment");
            this.Property(t => t.NoticeNewFriend).HasColumnName("user_settings_notice_new_friend");
            this.Property(t => t.Timezone).HasColumnName("user_settings_timezone");

            // Relationships
            this.HasOptional(t => t.UserAdministrator).WithRequired(t => t.User);
        }
    }
}
