namespace MirGames.Domain.Entities
{
    using System.Collections.Generic;

    public partial class geo_country
    {
        public geo_country()
        {
            this.geo_city = new List<geo_city>();
            this.geo_region = new List<geo_region>();
            this.geo_target = new List<geo_target>();
        }

        public int id { get; set; }
        public string name_ru { get; set; }
        public string name_en { get; set; }
        public string code { get; set; }
        public int sort { get; set; }
        public virtual ICollection<geo_city> geo_city { get; set; }
        public virtual ICollection<geo_region> geo_region { get; set; }
        public virtual ICollection<geo_target> geo_target { get; set; }
    }
}
