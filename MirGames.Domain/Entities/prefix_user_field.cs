namespace MirGames.Domain.Entities
{
    using System.Collections.Generic;

    public partial class user_field
    {
        public user_field()
        {
            this.user_field_value = new List<user_field_value>();
        }

        public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string pattern { get; set; }
        public virtual ICollection<user_field_value> user_field_value { get; set; }
    }
}
