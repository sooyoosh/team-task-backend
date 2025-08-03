namespace TeamTaskManager.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = default!;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
