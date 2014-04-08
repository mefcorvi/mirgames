namespace MirGames.Domain.Entities
{
    public partial class friend
    {
        public long user_from { get; set; }
        public long user_to { get; set; }
        public int status_from { get; set; }
        public int status_to { get; set; }
        public virtual User user { get; set; }
        public virtual User user1 { get; set; }
    }
}
