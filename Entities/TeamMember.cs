namespace TeamTaskManager.Entities
{
    public class TeamMember
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public string Role { get; set; } = "Member";
    }
}
