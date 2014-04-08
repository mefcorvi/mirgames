namespace MirGames.Domain.Entities
{
    using System;

    public partial class talk_user
    {
        public long talk_id { get; set; }
        public long user_id { get; set; }
        public Nullable<System.DateTime> date_last { get; set; }
        public int comment_id_last { get; set; }
        public int comment_count_new { get; set; }
        public Nullable<bool> talk_user_active { get; set; }
        public virtual talk talk { get; set; }
        public virtual User user { get; set; }
    }
}
