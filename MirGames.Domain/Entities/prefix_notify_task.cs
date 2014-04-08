namespace MirGames.Domain.Entities
{
    using System;

    public partial class notify_task
    {
        public long notify_task_id { get; set; }
        public string user_login { get; set; }
        public string user_mail { get; set; }
        public string notify_subject { get; set; }
        public string notify_text { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public Nullable<byte> notify_task_status { get; set; }
    }
}
