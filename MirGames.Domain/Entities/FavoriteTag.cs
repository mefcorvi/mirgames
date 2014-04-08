namespace MirGames.Domain.Entities
{
    public sealed class FavoriteTag
    {
        public long UserId { get; set; }
        
        public int TargetId { get; set; }
        
        public string TargetType { get; set; }
        
        public bool IsUser { get; set; }
        
        public string Text { get; set; }

        public User User { get; set; }
    }
}
