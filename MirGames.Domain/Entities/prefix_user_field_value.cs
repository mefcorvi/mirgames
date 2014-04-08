namespace MirGames.Domain.Entities
{
    using System;

    public partial class user_field_value
    {
        public long user_id { get; set; }
        public Nullable<int> field_id { get; set; }
        public string value { get; set; }
        public virtual User user { get; set; }
        public virtual user_field user_field { get; set; }
    }
}
