namespace MirGames.Domain.Entities
{
    using System;

    public partial class topic_photo
    {
        public int id { get; set; }
        public Nullable<long> topic_id { get; set; }
        public string path { get; set; }
        public string description { get; set; }
        public string target_tmp { get; set; }
        public virtual Topic topic { get; set; }
    }
}
